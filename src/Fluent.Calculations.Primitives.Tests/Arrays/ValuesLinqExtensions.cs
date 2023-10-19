using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Tests.Arrays
{
    public static class ValuesLinqExtensions
    {
        public static Number Sum(this IEnumerable<IValue> values)
            => Number.Of(values.Sum(value => value.Primitive));

    }
}