namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IEvaluationContext<T> : IEvaluationContext where T : class, IValueProvider, new()
{
    T ToResult();
}
