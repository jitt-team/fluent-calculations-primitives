namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string NaN = "NaN";
    private readonly IEvaluationResultsCache resultsCache;
    private readonly IMemberExpressionValueCapturer expressionValuesCapturer;
    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() : this(new MemberExpressionValueCapturer(), new EvaluationResultsCache()) { }

    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) : this() => calculationFunc = func;

    internal EvaluationContext(IMemberExpressionValueCapturer expressionCapturer, IEvaluationResultsCache resultsCache)
    {
        this.expressionValuesCapturer = expressionCapturer;
        this.resultsCache = resultsCache;
    }

    public TResult ToResult()
    {
        TResult result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (TResult)((IOrigin)result).MarkAsEndResult();
    }

    public virtual TResult Return() { return (TResult)new TResult().Default; }

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = NaN,
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = NaN)
            where ExpressionResultValue : class, IValue, new()
    {
        string lambdaExpressionBodyAdjusted = LamdaExpressionPrefixRemover.RemovePrefix(lambdaExpressionBody);

        if (!lambdaExpressionBody.Equals(NaN) &&
            resultsCache.ContainsKey(lambdaExpressionBodyAdjusted))
            return (ExpressionResultValue)resultsCache.GetByKey(lambdaExpressionBodyAdjusted);

        ExpressionResultValue value = EvaluateInternal(expression, name, lambdaExpressionBodyAdjusted);

        resultsCache.Add(lambdaExpressionBodyAdjusted, value);

        return value;
    }

    private ExpressionResultValue EvaluateInternal<ExpressionResultValue>(
       Expression<Func<ExpressionResultValue>> expression, string name, string expressionBody)
           where ExpressionResultValue : class, IValue, new()
    {
        ExpressionResultValue expressionResultValue = expression.Compile().Invoke();

        // TODO : Detect binary expressions and skip Expression Capture.
        MemberExpressionValues members = expressionValuesCapturer.Capture(expression);
        SyncParameterMemberNamesAndOrigin(members.Parameters);

        IValue[]
            parameterValues = members.Parameters.Select(capture => capture.Value).ToArray(),
            evaluationValues = SelectCachedEvaluationsValues(members.Evaluations),
            expressionArguments = parameterValues.Concat(evaluationValues).Distinct().ToArray();

        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda).WithArguments(expressionArguments);

        return (ExpressionResultValue)expressionResultValue.Make(MakeValueArgs.Compose(name, expressionNode, expressionResultValue.Primitive));
    }

    private IValue[] SelectCachedEvaluationsValues(CapturedEvaluation[] evaluations)
    {
        return evaluations.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaluation evaluation) => resultsCache.ContainsName(evaluation.Name);
        IValue GetCachedValue(CapturedEvaluation evaluation) => resultsCache.GetByName(evaluation.Name);
    }

    private void SyncParameterMemberNamesAndOrigin(CapturedParameter[] parameters)
    {
        foreach (CapturedParameter parameter in parameters)
        {
            ((IName)parameter.Value).Set(parameter.Name);
            ((IOrigin)parameter.Value).MarkAsInput();
        }
    }
}