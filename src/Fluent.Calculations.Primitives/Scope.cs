using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public partial class Scope<TResult> where TResult : class, IValue, new()
{
    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();
    private readonly EvaluationCache evaluationCache = new EvaluationCache();

    private Func<Scope<TResult>, TResult>? calculationFunc;

    public Scope() { }

    public Scope(Func<Scope<TResult>, TResult> func) => calculationFunc = func;

    public TResult Calculate() => calculationFunc?.Invoke(this) ?? Return();

    public virtual TResult Return() { return (TResult)new TResult().Default; }

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "Undefined",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "Undefined") 
            where ExpressionResultValue : class, IValue, new()
    {
        string lambdaExpressionBodyAdjusted = LamdaExpressionPrefixRemover.RemovePrefix(lambdaExpressionBody);

        if (evaluationCache.ContainsKey(lambdaExpressionBodyAdjusted))
            return (ExpressionResultValue)evaluationCache.GetByKey(lambdaExpressionBodyAdjusted);

        ExpressionResultValue expresionResult = expression.Compile().Invoke();
        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, lambdaExpressionBodyAdjusted);

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(expresionResult.Expression.Arguments);

        IValue value = expresionResult.ToExpressionResult(CreateValueArgs.Compose(name, expressionNode, expresionResult.PrimitiveValue));

        evaluationCache.Add(lambdaExpressionBodyAdjusted, value);

        return (ExpressionResultValue)value;
    }
}