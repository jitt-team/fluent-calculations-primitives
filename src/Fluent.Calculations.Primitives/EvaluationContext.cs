namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string NaN = "NaN";
    private readonly EvaluationResultCache resultCache = new EvaluationResultCache();
    private readonly IExpressionCapturer expressionCapturer;
    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() : this(new ExpressionCapturer()) { }
    
    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) : this() => calculationFunc = func;

    internal EvaluationContext(IExpressionCapturer expressionCapturer) => this.expressionCapturer = expressionCapturer;

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
        ExpressionResultValue expressionResultValue = expression.Compile().Invoke();
        ExpressionCaptureResult captureResult = expressionCapturer.Capture(expression);
        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda);

        IValue[] inputValues = GetSyncedNameInputValues(captureResult.InputMembers);
        IValue[] evaluationValues = ResolveEvaluationMembersToValues(captureResult.EvaluationMembers);
        IValue[] arguments = inputValues.Concat(evaluationValues).Distinct().ToArray();

        expressionNode.WithArguments(arguments);

        return (ExpressionResultValue)expressionResultValue.Create(CreateValueArgs.Build(name, expressionNode, expressionResultValue.Primitive));
    }

    private IValue[] ResolveEvaluationMembersToValues(CapturedEvaulationMember[] evaluationPointers)
    {
        return evaluationPointers.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaulationMember pointer) => resultCache.ContainsName(pointer.Name);
        IValue GetCachedValue(CapturedEvaulationMember pointer) => resultCache.GetByName(pointer.Name);
    }

    private IValue[] GetSyncedNameInputValues(CapturedInputMember[] inputParameters)
    {
        foreach (CapturedInputMember inputParameter in inputParameters)
        {
            ((IName)inputParameter.Value).Set(inputParameter.Name);
            ((IOrigin)inputParameter.Value).MarkAsInput();
        }

        return inputParameters.Select(capture => capture.Value).ToArray();
    }
}