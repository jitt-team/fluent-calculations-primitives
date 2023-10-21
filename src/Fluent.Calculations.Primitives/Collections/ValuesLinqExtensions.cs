namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using static Fluent.Calculations.Primitives.Expressions.AggregateExpressionHandler;

public static class ValuesLinqExtensions
{
    public static TSource Sum<TSource>(this IValues<TSource> values) where TSource : class, IValue, new() => 
        HandleAggregate(values, Enumerable.Sum);

    public static TSource Average<TSource>(this IValues<TSource> values) where TSource : class, IValue, new() => 
        HandleAggregate(values, Enumerable.Average);

    public static TSource Min<TSource>(this IValues<TSource> values) where TSource : class, IValue, new() =>
        HandleAggregate(values, Enumerable.Min);

    public static TSource Max<TSource>(this IValues<TSource> values) where TSource : class, IValue, new() =>
        HandleAggregate(values, Enumerable.Max);

}

