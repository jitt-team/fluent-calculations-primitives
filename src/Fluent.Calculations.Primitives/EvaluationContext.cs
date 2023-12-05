﻿namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/class/*' />
public class EvaluationContext<T> : IEvaluationContext<T> where T : class, IValueProvider, new()
{
    private readonly EvaluationOptions options;
    private readonly IValuesCache valuesCache;
    private readonly IMemberExpressionValueCapturer memberCapturer;
    private readonly Func<EvaluationContext<T>, T>? calculationFunc;

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/ctor/*' />
    public EvaluationContext() : this(EvaluationOptions.Default) { }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/ctor-options/*' />
    public EvaluationContext(EvaluationOptions options) :
        this(new ValuesCache(), new MemberExpressionValueCapturer()) => this.options = options;

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/ctor-func/*' />
    public EvaluationContext(Func<EvaluationContext<T>, T> func) :
        this(EvaluationOptions.Default) => calculationFunc = func;

    internal EvaluationContext(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer)
    {
        this.options = EvaluationOptions.Default;
        this.memberCapturer = memberCapturer;
        this.valuesCache = valuesCache;
    }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/method-ToResult/*' />
    public T ToResult()
    {
        valuesCache.Clear();

        T result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (T)((IOrigin)result).AsResult();
    }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/method-Return/*' />
    public virtual T Return() { return (T)new T().MakeDefault(); }

    public TValue Evaluate<TCase, TValue>(Func<SwitchExpression<TCase, TValue>.ResultEvaluator> getSwitchResultFunc, [CallerMemberName] string name = StringConstants.NaN)
            where TCase : struct, Enum
            where TValue : class, IValueProvider, new()
    {
        if (!name.Equals(StringConstants.NaN) && valuesCache.ContainsKey(name))
            return (TValue)valuesCache.GetByKey(name);

        return getSwitchResultFunc().GetResult(name);
    }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/method-Evaluate/*' />
    public TValue Evaluate<TValue>(
    Expression<Func<TValue>> lambdaExpression,
    [CallerMemberName] string name = StringConstants.NaN,
    [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN)
        where TValue : class, IValueProvider, new()
    {
        if (!name.Equals(StringConstants.NaN) && valuesCache.ContainsKey(name))
            return (TValue)valuesCache.GetByKey(name);

        TValue result = EvaluateInternal(lambdaExpression, name, RemoveLambdaPrefix(lambdaExpressionBody));

        if (!name.Equals(StringConstants.NaN))
            valuesCache.Add(name, result);

        return result;

        static string RemoveLambdaPrefix(string body) => body.Replace("() => ", "");
    }

    private TValue EvaluateInternal<TValue>(
       Expression<Func<TValue>> lambdaExpression, string name, string expressionBody)
           where TValue : class, IValueProvider, new()
    {
        TValue result = lambdaExpression.Compile().Invoke();

        CapturedExpressionMembers members = memberCapturer.Capture(lambdaExpression);
        MarkValuesAsParameters(members.Parameters);

        IEnumerable<IValueProvider>
            parameterValues = members.Parameters.Select(capture => capture.Value),
            evaluationValues = SelectCachedEvaluationsValues(members.Evaluations),
            expressionArguments = parameterValues.Concat(evaluationValues);

        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda).WithArguments(expressionArguments);

        return (TValue)result.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, result.Primitive, ValueOriginType.Evaluation));
    }

    private IValueProvider[] SelectCachedEvaluationsValues(CapturedEvaluationMember[] evaluations)
    {
        return evaluations.Where(IsCached).Select(GetCachedValue).ToArray();
        bool IsCached(CapturedEvaluationMember evaluation) => valuesCache.ContainsName(evaluation.MemberName);
        IValueProvider GetCachedValue(CapturedEvaluationMember evaluation) => valuesCache.GetByName(evaluation.MemberName);
    }
    private void MarkValuesAsParameters(CapturedParameterMember[] parameters)
    {
        foreach (CapturedParameterMember parameter in parameters)
        {
            IOrigin parameterOrigin = ((IOrigin)parameter.Value);
            if (options.AlwaysReadNamesFromExpressions || !parameterOrigin.IsSet)
                parameterOrigin.MarkAsParameter(parameter.MemberName);
        }
    }
}