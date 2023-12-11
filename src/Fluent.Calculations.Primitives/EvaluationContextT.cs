namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.Expressions;

public class EvaluationContext<T> : EvaluationContext, IEvaluationContext<T> where T : class, IValueProvider, new()
{
    private readonly Func<EvaluationContext<T>, T>? calculationFunc;

    internal EvaluationContext(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer) : base(valuesCache, memberCapturer)
    { }

    public EvaluationContext() : base() { }

    public EvaluationContext(EvaluationOptions options) : base(options) { }

    public EvaluationContext(string scope) : base(scope) { }

    internal EvaluationContext(IValuesCache valuesCache, IMemberExpressionValueCapturer memberCapturer, IValueArgumentsSelector selector) :
        base(valuesCache, memberCapturer, selector)
    { }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/ctor-func/*' />
    public EvaluationContext(Func<EvaluationContext<T>, T> func) : base(EvaluationOptions.Default) => calculationFunc = func;

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/method-ToResult/*' />
    public T ToResult()
    {
        ClearValuesCache();

        T result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (T)((IOrigin)result).AsResult();
    }

    /// <include file="Docs/IntelliSense.xml" path='docs/members[@name="EvaluationContext"]/method-Return/*' />
    public virtual T Return() { return (T)new T().MakeDefault(); }
}
