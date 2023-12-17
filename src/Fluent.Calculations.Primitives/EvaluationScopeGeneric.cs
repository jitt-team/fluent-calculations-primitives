namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.Expressions;

public class EvaluationScope<T> : EvaluationScope, IEvaluationScope<T> where T : class, IValueProvider, new()
{
    private readonly Func<EvaluationScope<T>, T>? calculationFunc;

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer) : base(valuesCache, memberCapturer)
    { }

    public EvaluationScope() : base() { }

    public EvaluationScope(EvaluationOptions options) : base(options) { }

    public EvaluationScope(string scope) : base(scope) { }

    internal EvaluationScope(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer, IValueArgumentsSelector selector) :
        base(valuesCache, memberCapturer, selector)
    { }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationScope"]/ctor-func/*' />
    public EvaluationScope(Func<EvaluationScope<T>, T> func) : base(EvaluationOptions.Default) => calculationFunc = func;

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationScope"]/method-ToResult/*' />
    public T ToResult()
    {
        ClearValuesCache();

        T result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (T)((IOrigin)result).AsResult();
    }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationScope"]/method-Return/*' />
    public virtual T Return() { return (T)new T().MakeDefault(); }
}
