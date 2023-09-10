namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<TResult> where TResult : class, IValue, new()
{
    private const string NaN = "NaN";
    private readonly ExpressionMembersCapturer expressionPartTranslator = new ExpressionMembersCapturer();
    private readonly EvaluationResultCache resultCache = new EvaluationResultCache();
    private Func<EvaluationContext<TResult>, TResult>? calculationFunc;

    public EvaluationContext() { }

    public EvaluationContext(Func<EvaluationContext<TResult>, TResult> func) => calculationFunc = func;

    public TResult ToResult()
    {
        TResult result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (TResult)((IValueOrigin)result).MarkAsEndResult();
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
        ExpressionMembersCaptureResult captureResult = expressionPartTranslator.Capture(expression);
        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda);

        IValue[] inputValues = GetSyncedNameInputParameters(captureResult.InputParameters);
        IValue[] evaluationValues = ResolveEvaluationPointersToValues(captureResult.EvaluationPointers);
        IValue[] arguments = inputValues.Concat(evaluationValues).Distinct().ToArray();

        expressionNode.WithArguments(arguments);

        return (ExpressionResultValue)expressionResultValue.Create(CreateValueArgs.Build(name, expressionNode, expressionResultValue.Primitive));
    }

    private IValue[] ResolveEvaluationPointersToValues(PointerToEvaulationCapture[] evaluationPointers)
    {
        return evaluationPointers.Where(IsCached).Select(GetFromCache).ToArray();
        bool IsCached(PointerToEvaulationCapture pointer) => resultCache.ContainsName(pointer.Name);
        IValue GetFromCache(PointerToEvaulationCapture pointer) => resultCache.GetByName(pointer.Name);
    }

    private IValue[] GetSyncedNameInputParameters(InputParameterCapture[] inputParameters)
    {
        foreach (InputParameterCapture inputParameter in inputParameters)
        {
            ((IName)inputParameter.Value).Set(inputParameter.Name);
            ((IValueOrigin)inputParameter.Value).MarkAsInput();
        }

        return inputParameters.Select(capture => capture.Value).ToArray();
    }
}