using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;

namespace Fluent.Calculations.Primitives.Tests.Arrays
{
    public static class ValuesLinqExtensions
    {
        public static Values<ValueType> Sum<ValueType>(this IEnumerable<IValue> values) where ValueType : IValue
            => Number.Of(values.Sum(value => value.Primitive));

    }
}