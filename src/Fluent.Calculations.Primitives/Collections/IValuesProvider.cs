namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IValuesProvider/class/*' />
public interface IValuesProvider<T> : IReadOnlyCollection<T>, IValueProvider where T : class, IValueProvider, new()
{
    IValueProvider MakeOfThisElementType(MakeValueArgs args);
}

