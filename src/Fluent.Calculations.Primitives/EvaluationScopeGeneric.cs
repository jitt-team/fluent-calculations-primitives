namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.Expressions;

/// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/class/*' />
public class EvaluationScope<T> : EvaluationScope, IEvaluationScope<T> where T : class, IValueProvider, new()
{
    private readonly Func<EvaluationScope<T>, T>? calculationFunc;

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer) : base(valuesCache, memberCapturer)
    { }

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/ctor/*' />
    public EvaluationScope() : base() { }

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/ctor-options/*' />
    public EvaluationScope(EvaluationOptions options) : base(options) { }

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/ctor-scope/*' />
    public EvaluationScope(string scope) : base(scope) { }

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer, IValueArgumentsSelector selector) :
        base(valuesCache, memberCapturer, selector)
    { }

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/ctor-func/*' />
    public EvaluationScope(Func<EvaluationScope<T>, T> func) : base(EvaluationOptions.Default) => calculationFunc = func;

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/method-ToResult/*' />
    public T ToResult()
    {
        ClearValuesCache();

        T result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (T)((IOrigin)result).AsResult();
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="EvaluationScopeGeneric"]/method-Return/*' />
    public virtual T Return() { return (T)new T().MakeDefault(); }
}
