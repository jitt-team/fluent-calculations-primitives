namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='*/IEvaluationScopeGeneric/interface/*'/>
public interface IEvaluationScope<out T> : IEvaluationScope where T : class, IValueProvider, new()
{
    T ToResult();
}
