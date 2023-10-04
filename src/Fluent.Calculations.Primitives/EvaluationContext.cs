﻿namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class EvaluationContext<ResultValueType> : IEvaluationContext<ResultValueType> where ResultValueType : class, IValue, new()
{
    private readonly IValuesCache valuesCache;
    private readonly IMemberExpressionValueCapturer memberCapturer;
    private Func<EvaluationContext<ResultValueType>, ResultValueType>? calculationFunc;

    public EvaluationContext() : this(new ValuesCache(), new MemberExpressionValueCapturer()) { }

    public EvaluationContext(Func<EvaluationContext<ResultValueType>, ResultValueType> func) : this() => calculationFunc = func;

    internal EvaluationContext(IValuesCache resultsCache, IMemberExpressionValueCapturer expressionCapturer)
    {
        this.memberCapturer = expressionCapturer;
        this.valuesCache = resultsCache;
    }

    public ResultValueType ToResult()
    {
        ResultValueType result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (ResultValueType)((IOrigin)result).AsResult();
    }

    public virtual ResultValueType Return() { return (ResultValueType)new ResultValueType().Default; }

    public ValueType Evaluate<ValueType>(
        Expression<Func<ValueType>> lambdaExpression,
        [CallerMemberName] string name = Constants.NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = Constants.NaN)
            where ValueType : class, IValue, new()
    {
        if (!name.Equals(Constants.NaN) && valuesCache.ContainsKey(name))
            return (ValueType)valuesCache.GetByKey(name);

        ValueType result = EvaluateInternal(lambdaExpression, name, RemoveLambdaPrefix(lambdaExpressionBody));

        if (!name.Equals(Constants.NaN))
            valuesCache.Add(name, result);

        return result;

        string RemoveLambdaPrefix(string body) => body.Replace("() => ", "");
    }

    private ValueType EvaluateInternal<ValueType>(
       Expression<Func<ValueType>> lambdaExpression, string name, string expressionBody)
           where ValueType : class, IValue, new()
    {
        ValueType result = lambdaExpression.Compile().Invoke();

        ExpressionNode expressionNode;

        CapturedExpressionMembers members = memberCapturer.Capture(lambdaExpression);
        MarkValuesAsParameters(members.Parameters);

        IEnumerable<IValue>
            parameterValues = members.Parameters.Select(capture => capture.Value),
            evaluationValues = SelectCachedEvaluationsValues(members.Evaluations),
            expressionArguments = parameterValues.Concat(evaluationValues);

        expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda).WithArguments(expressionArguments);

        return (ValueType)result.Make(MakeValueArgs.Compose(name, expressionNode, result.Primitive));
    }

    private IValue[] SelectCachedEvaluationsValues(CapturedEvaluationMember[] evaluations)
    {
        return evaluations.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaluationMember evaluation) => valuesCache.ContainsName(evaluation.MemberName);
        IValue GetCachedValue(CapturedEvaluationMember evaluation) => valuesCache.GetByName(evaluation.MemberName);
    }

    private void MarkValuesAsParameters(CapturedParameterMember[] parameters)
    {
        foreach (CapturedParameterMember parameter in parameters)
            ((IOrigin)parameter.Value).MarkAsParameter(parameter.MemberName);
    }
}