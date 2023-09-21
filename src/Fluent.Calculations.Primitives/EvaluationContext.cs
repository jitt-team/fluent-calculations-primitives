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
    private readonly IExpressionValuesCapturer expressionValuesCapturer;
    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() : this(new ExpressionValuesCapturer(), new EvaluationResultsCache()) { }
    
    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) : this() => calculationFunc = func;

    internal EvaluationContext(IExpressionValuesCapturer expressionCapturer, IEvaluationResultsCache resultsCache)
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
        CapturedExpressionValues captureResult = expressionValuesCapturer.Capture(expression);
        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda);

        IValue[] parameterValues = GetSyncedNameInputValues(captureResult.Parameters);
        IValue[] evaluationValues = SelectCachedEvaluationsValues(captureResult.Evaluations);
        IValue[] expressionArguments = parameterValues.Concat(evaluationValues).Distinct().ToArray();

        expressionNode.WithArguments(expressionArguments);

        return (ExpressionResultValue)expressionResultValue.Make(MakeValueArgs.Compose(name, expressionNode, expressionResultValue.Primitive));
    }

    private IValue[] SelectCachedEvaluationsValues(CapturedEvaluation[] evaluations)
    {
        return evaluations.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaluation evaluation) => resultsCache.ContainsName(evaluation.Name);
        IValue GetCachedValue(CapturedEvaluation evaluation) => resultsCache.GetByName(evaluation.Name);
    }

    private IValue[] GetSyncedNameInputValues(CapturedParameter[] parameters)
    {
        foreach (CapturedParameter parameter in parameters)
        {
            ((IName)parameter.Value).Set(parameter.Name);
            ((IOrigin)parameter.Value).MarkAsInput();
        }

        return parameters.Select(capture => capture.Value).ToArray();
    }
}