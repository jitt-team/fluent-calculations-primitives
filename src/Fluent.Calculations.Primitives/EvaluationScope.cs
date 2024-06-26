﻿namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/EvaluationScope/class/*'/>
public class EvaluationScope : IEvaluationScope
{
    private readonly EvaluationOptions options;
    private readonly IValuesCache valuesCache;
    private readonly IMemberExpressionValueCapturer memberCapturer;
    private readonly IValueArgumentsSelector valueArgumentsSelector;

    /// <include file="Docs.xml" path='*/EvaluationScope/ctor/*'/>
    public EvaluationScope() : this(new ValuesCache(), new MemberExpressionValueCapturer()) =>
        options = new EvaluationOptions { Scope = GetType().Name };

    /// <include file="Docs.xml" path='*/EvaluationScope/ctor-options/*'/>
    public EvaluationScope(EvaluationOptions options) :
        this(new ValuesCache(), new MemberExpressionValueCapturer()) => this.options = options;

    /// <include file="Docs.xml" path='*/EvaluationScope/ctor-scope/*'/>
    public EvaluationScope(string scope) : this(new EvaluationOptions { Scope = scope }) { }

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer) :
        this(valuesCache, memberCapturer, new ValueArgumentsSelector())
    { }

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer, IValueArgumentsSelector selector)
    {
        this.options = EvaluationOptions.Default;
        this.memberCapturer = memberCapturer;
        this.valuesCache = valuesCache;
        this.valueArgumentsSelector = selector;
    }

    /// <include file="Docs.xml" path='*/EvaluationScope/Create/*'/>
    public static EvaluationScope Create([CallerMemberName] string scope = StringConstants.NaN) =>
     new(new EvaluationOptions { AlwaysReadNamesFromExpressions = true, Scope = scope });

    /// <include file="Docs.xml" path='*/EvaluationScope/Evaluate-switch/*'/>
    public TValue Evaluate<TCase, TValue>(Func<SwitchExpression<TCase, TValue>.ResultEvaluator> getResultEvaluatorFunc, [CallerMemberName] string name = StringConstants.NaN)
            where TCase : struct, Enum
            where TValue : class, IValueProvider, new()
    {
        if (!name.Equals(StringConstants.NaN) && valuesCache.ContainsKey(name))
            return (TValue)valuesCache.GetByKey(name);

        return getResultEvaluatorFunc().GetResult(name);
    }

    /// <include file="Docs.xml" path='*/EvaluationScope/Evaluate/*'/>
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

    /// <include file="Docs.xml" path='*/EvaluationScope/ClearCache/*'/>
    protected void ClearCache() => valuesCache.Clear();

    private TValue EvaluateInternal<TValue>(
       Expression<Func<TValue>> lambdaExpression, string name, string expressionBody)
           where TValue : class, IValueProvider, new()
    {
        TValue result = lambdaExpression.Compile().Invoke();

        CapturedExpressionMembers members = memberCapturer.Capture(lambdaExpression);
        MarkValuesAsParameters(members.Parameters);

        IEnumerable<IValue>
            parameterValues = members.Parameters.Select(capture => capture.Value),
            evaluationValues = SelectCachedEvaluationsValues(members.Evaluations),
            capturedExpressionArguments = parameterValues.Concat(evaluationValues),
            resultArguments = valueArgumentsSelector.Select(result),
            missingArguments = resultArguments.Where(a => !capturedExpressionArguments.Any(e => e.Name.Equals(a.Name))),
            expressionArguments = capturedExpressionArguments.Concat(missingArguments);

        ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Lambda).WithArguments(expressionArguments);

        return (TValue)result.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, result.Primitive, ValueOriginType.Evaluation, options.Scope));
    }

    private IValue[] SelectCachedEvaluationsValues(CapturedEvaluationMember[] evaluations)
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
                parameterOrigin.MarkAsParameter(parameter.MemberName, options.Scope);
        }
    }
}