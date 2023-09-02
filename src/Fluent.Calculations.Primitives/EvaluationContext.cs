namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string NaN = "NaN";

    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();
    private readonly ExpressionResultCache resultCache = new ExpressionResultCache();

    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() { }

    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) => calculationFunc = func;

    public TResult ToResult()
    {
        TResult result;

        if (calculationFunc != null)
            result = calculationFunc.Invoke(this);
        else
            result = Return();

        return ((IEndResult)result).AsEndResult<TResult>();
    }

    public virtual TResult Return() { return (TResult)new TResult().Default; }

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "NaN",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "NaN")
            where ExpressionResultValue : class, IValue, new()
    {
        string lambdaExpressionBodyAdjusted = LamdaExpressionPrefixRemover.RemovePrefix(lambdaExpressionBody);

        if (!lambdaExpressionBody.Equals(NaN) &&
            resultCache.ContainsKey(lambdaExpressionBodyAdjusted))
            return (ExpressionResultValue)resultCache.GetByKey(lambdaExpressionBodyAdjusted);

        ExpressionResultValue value = EvaluateInternal(expression, name, lambdaExpressionBodyAdjusted);

        resultCache.Add(lambdaExpressionBodyAdjusted, value);

        return value;
    }

    private ExpressionResultValue EvaluateInternal<ExpressionResultValue>(
       Expression<Func<ExpressionResultValue>> expression, string name, string expressionBody)
           where ExpressionResultValue : class, IValue, new()
    {
        ExpressionResultValue plainResult = expression.Compile().Invoke();
        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, expressionBody);

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(plainResult.Expression.Arguments);

        return (ExpressionResultValue)plainResult.Create(CreateValueArgs.Create(name, expressionNode, plainResult.PrimitiveValue));
    }
}