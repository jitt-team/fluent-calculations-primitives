namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;

public static class ValuesLinqExtensions
{
    public static Number Sum<ValueType>(this IEnumerable<ValueType> values) where ValueType : IValue
        => Number.Of(values.Sum(value => value.Primitive));

}
