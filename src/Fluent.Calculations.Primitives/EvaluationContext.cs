using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string Undefined = "Undefined";

    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();
    private readonly EvaluationCache evaluationCache = new EvaluationCache();

    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() { }

    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) => calculationFunc = func;

    public TResult Calculate() => calculationFunc?.Invoke(this) ?? Return();

    public virtual TResult Return() { return (TResult)new TResult().Default; }

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "Undefined",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "Undefined")
            where ExpressionResultValue : class, IValue, new()
    {
        string lambdaExpressionBodyAdjusted = LamdaExpressionPrefixRemover.RemovePrefix(lambdaExpressionBody);

        if (!lambdaExpressionBody.Equals(Undefined) &&
            evaluationCache.ContainsKey(lambdaExpressionBodyAdjusted))
            return (ExpressionResultValue)evaluationCache.GetByKey(lambdaExpressionBodyAdjusted);

        ExpressionResultValue value = EvaluateInternal(expression, name, lambdaExpressionBodyAdjusted);

        evaluationCache.Add(lambdaExpressionBodyAdjusted, value);

        return value;
    }

    public ExpressionResultValue EvaluateInternal<ExpressionResultValue>(
       Expression<Func<ExpressionResultValue>> expression, string name, string lambdaExpressionBodyAdjusted)
           where ExpressionResultValue : class, IValue, new()
    {
        ExpressionResultValue plainResult = expression.Compile().Invoke();
        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, lambdaExpressionBodyAdjusted);

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(plainResult.Expression.Arguments);

        return (ExpressionResultValue)plainResult.Compose(CreateValueArgs.Compose(name, expressionNode, plainResult.PrimitiveValue));
    }
}