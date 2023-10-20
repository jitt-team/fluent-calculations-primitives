namespace Fluent.Calculations.Primitives.Collections;

using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public static class ValuesLinqExtensions
{
    public static CollectionElementType Sum<CollectionType, CollectionElementType>(this CollectionType collection)
        where CollectionElementType : class, IValue, new()
        where CollectionType : class, IEnumerable<CollectionElementType>, IValues<CollectionElementType>, new()

        => BinaryExpressionHandler.HandleAggregate<CollectionType, CollectionElementType>(collection, values => values.Sum(value => value.Primitive), "Sum");

}
