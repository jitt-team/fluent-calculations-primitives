namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/EvaluationScopeGeneric/class/*'/>
public abstract class EvaluationScope<T> : EvaluationScope, IEvaluationScope<T> where T : class, IValueProvider, new()
{
    private readonly Func<EvaluationScope<T>, T>? calculationFunc;

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/ctor/*'/>
    protected EvaluationScope() : base() { }

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/ctor-options/*'/>
    protected EvaluationScope(EvaluationOptions options) : base(options) { }

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/ctor-scope/*'/>
    protected EvaluationScope(string scope) : base(scope) { }

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/ctor-func/*'/>
    protected EvaluationScope(Func<EvaluationScope<T>, T> func) : base(EvaluationOptions.Default) => calculationFunc = func;

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/ToResult/*'/>
    public T ToResult()
    {
        ClearCache();

        T result = calculationFunc != null ?
             calculationFunc.Invoke(this) :
             Return();

        return (T)((IOrigin)result).AsResult();
    }

    /// <include file="Docs.xml" path='*/EvaluationScopeGeneric/Return/*'/>
    public virtual T Return() { return (T)new T().MakeDefault(); }
}
