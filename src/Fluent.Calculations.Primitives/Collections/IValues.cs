namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IValues<T> : IReadOnlyCollection<T>, IValue where T : class, IValue, new()
{
    IValue MakeOfThisElementType(MakeValueArgs args);
}

