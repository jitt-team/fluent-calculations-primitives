using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public abstract class Calculation<TResult> : IValue where TResult : class, IValue, new()
{
    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();

    public Calculation()
    { }

    Dictionary<string, IValue> valueAmountResults = new Dictionary<string, IValue>();

    public List<string> Tags => new List<string>();

    public decimal PrimitiveValue => Is(() => Calculate(), Expression.GetType().Name).PrimitiveValue;

    public ExpressionNode Expression => Is(() => Calculate(), Expression.GetType().Name).Expression;

    public string Name { get; set; } = "TODO";

    public bool IsConstant => false;

    TagsList IValue.Tags => Return().Tags;

    public TResult Calculate() => Return();

    public abstract TResult Return();

    public IValue ToExpressionResult(CreateValueArgs args) => new TResult().ToExpressionResult(args);

    protected ExpressionResultValue Is<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "") where ExpressionResultValue : class, IValue
    {
        string lambdaExpressionBodyAdjusted = AdjustLambdaPrefix(lambdaExpressionBody);
        string prefixedName = $"{name} = {lambdaExpressionBodyAdjusted}";
        if (valueAmountResults.TryGetValue(prefixedName, out IValue cachedValue))
            return (ExpressionResultValue)cachedValue;

        ExpressionResultValue result = expression.Compile().Invoke();
        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, lambdaExpressionBodyAdjusted);

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(result.Expression.Arguments);

        foreach (IValue arg in expressionNode.Arguments)
            if (!valueAmountResults.ContainsKey(arg.Name))
                valueAmountResults.Add(arg.Name, arg);

        IValue value = result.ToExpressionResult(CreateValueArgs.Compose(prefixedName, expressionNode, result.PrimitiveValue));

        valueAmountResults.Add(prefixedName, value);

        return (ExpressionResultValue)value;

        string AdjustLambdaPrefix(string body) => body.Replace("() => ", "");
    }


}