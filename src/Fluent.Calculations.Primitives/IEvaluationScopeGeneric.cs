namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IEvaluationScope<out T> : IEvaluationScope where T : class, IValueProvider, new()
{
    T ToResult();
}
