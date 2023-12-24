namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IEvaluationScopeGeneric/interface/*'/>
public interface IEvaluationScope<out T> : IEvaluationScope where T : class, IValueProvider, new()
{
    /// <include file="Docs.xml" path='*/IEvaluationScopeGeneric/ToResult/*'/>
    T ToResult();
}
