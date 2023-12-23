namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='docs/members[@name="IEvaluationScopeGeneric"]/interface/*' />
public interface IEvaluationScope<out T> : IEvaluationScope where T : class, IValueProvider, new()
{
    T ToResult();
}
