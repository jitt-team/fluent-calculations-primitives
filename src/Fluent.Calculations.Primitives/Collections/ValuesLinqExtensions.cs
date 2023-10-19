namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;

public static class ValuesLinqExtensions
{
    public static Number Sum(this IEnumerable<IValue> values) 
        => Number.Of(values.Sum(value => value.Primitive));

}
