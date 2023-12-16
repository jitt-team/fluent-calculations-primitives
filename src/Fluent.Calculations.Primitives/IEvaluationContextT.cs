namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IEvaluationScope<T> : IEvaluationScope where T : class, IValueProvider, new()
{
    T ToResult();
}
