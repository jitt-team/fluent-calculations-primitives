using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Scope<TResult> where TResult : class, IValue, new()
{
    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();
    private readonly Dictionary<string, IValue> valueAmountResults = new Dictionary<string, IValue>();
    private Func<Scope<TResult>, TResult>? calculationFunc;

    public Scope() { }

    public Scope(Func<Scope<TResult>, TResult> func) => calculationFunc = func;

    public TResult Calculate() => calculationFunc?.Invoke(this) ?? Return();

    public virtual TResult Return() { return default(TResult); }

    public IValue ToExpressionResult(CreateValueArgs args) => new TResult().ToExpressionResult(args);

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "") where ExpressionResultValue : class, IValue
    {
        string lambdaExpressionBodyAdjusted = LamdaExpressionPrefixRemover.RemovePrefix(lambdaExpressionBody);

        if (valueAmountResults.TryGetValue(name, out IValue? cachedValue))
            return (ExpressionResultValue)cachedValue;

        ExpressionResultValue result = expression.Compile().Invoke();
        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, lambdaExpressionBodyAdjusted);

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(result.Expression.Arguments);

        foreach (IValue arg in expressionNode.Arguments)
            if (!valueAmountResults.ContainsKey(arg.Name))
                valueAmountResults.Add(arg.Name, arg);

        IValue value = result.ToExpressionResult(CreateValueArgs.Compose(name, expressionNode, result.PrimitiveValue));

        valueAmountResults.Add(name, value);

        return (ExpressionResultValue)value;
    }

    internal class LamdaExpressionPrefixRemover
    {
        public static string RemovePrefix(string body) => body.Replace("() => ", "");
    }
}