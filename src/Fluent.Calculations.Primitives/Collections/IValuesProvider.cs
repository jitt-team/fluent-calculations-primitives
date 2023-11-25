namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IValuesProvider<T> : IReadOnlyCollection<T>, IValueProvider where T : class, IValueProvider, new()
{
    IValueProvider MakeOfThisElementType(MakeValueArgs args);
}

