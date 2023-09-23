namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string NaN = "NaN";
    private readonly IValuesCache evaluationCache;
    private readonly IMemberExpressionValueCapturer expressionValuesCapturer;
    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() : this(new MemberExpressionValueCapturer(), new ValuesCache()) { }

    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) : this() => calculationFunc = func;

    internal EvaluationContext(IMemberExpressionValueCapturer expressionCapturer, IValuesCache resultsCache)
    {
        this.expressionValuesCapturer = expressionCapturer;
        this.evaluationCache = resultsCache;
    }

    public TResult ToResult()
    {
        TResult result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (TResult)((IOrigin)result).AsResult();
    }

    public virtual TResult Return() { return (TResult)new TResult().Default; }

    public ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> lambdaExpression,
        [CallerMemberName] string name = NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = NaN)
            where ExpressionResultValue : class, IValue, new()
    {
        if (!name.Equals(NaN) && evaluationCache.ContainsKey(name))
            return (ExpressionResultValue)evaluationCache.GetByKey(name);

        ExpressionResultValue result = EvaluateInternal(lambdaExpression, name, RemoveLambdaPrefix(lambdaExpressionBody));

        if (!name.Equals(NaN))
            evaluationCache.Add(name, result);

        return result;

        string RemoveLambdaPrefix(string body) => body.Replace("() => ", "");
    }

    private ExpressionResultValue EvaluateInternal<ExpressionResultValue>(
       Expression<Func<ExpressionResultValue>> lambdaExpression, string name, string expressionBody)
           where ExpressionResultValue : class, IValue, new()
    {
        ExpressionResultValue result = lambdaExpression.Compile().Invoke();

        ExpressionNode expressionNode;

        MemberExpressionValues members = expressionValuesCapturer.Capture(lambdaExpression);
        MarkValuesAsParameters(members.Parameters);

        IEnumerable<IValue>
            parameterValues = members.Parameters.Select(capture => capture.Value),
            evaluationValues = SelectCachedEvaluationsValues(members.Evaluations),
            expressionArguments = parameterValues.Concat(evaluationValues);

        expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda).WithArguments(expressionArguments);

        return (ExpressionResultValue)result.Make(MakeValueArgs.Compose(name, expressionNode, result.Primitive));
    }

    private IValue[] SelectCachedEvaluationsValues(CapturedEvaluation[] evaluations)
    {
        return evaluations.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaluation evaluation) => evaluationCache.ContainsName(evaluation.Name);
        IValue GetCachedValue(CapturedEvaluation evaluation) => evaluationCache.GetByName(evaluation.Name);
    }

    private void MarkValuesAsParameters(CapturedParameter[] parameters)
    {
        foreach (CapturedParameter parameter in parameters)
            ((IOrigin)parameter.Value).MarkAsParameter(parameter.Name);
    }
}