namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public static class ValuesLinqExtensions
{
    public static CollectionElementType Sum<CollectionElementType>(this IValues<CollectionElementType> collection)
        where CollectionElementType : class, IValue, new()      
        => AggregateExpressionHandler.HandleAggregate(collection, values => collection.Primitive, "Sum");

    public static CollectionElementType Average<CollectionElementType>(this IValues<CollectionElementType> collection)
        where CollectionElementType : class, IValue, new()
        => AggregateExpressionHandler.HandleAggregate(collection, values => values.Average(value => value.Primitive), "Average");
}
