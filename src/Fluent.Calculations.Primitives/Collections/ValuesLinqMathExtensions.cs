namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using static Fluent.Calculations.Primitives.Expressions.CollectionExpressionHandler;

public static class ValuesLinqMathExtensions
{
    public static TSource Sum<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => Handle(source, Enumerable.Sum);

    public static TSource Average<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => Handle(source, Enumerable.Average);

    public static TSource Min<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => Handle(source, Enumerable.Min);

    public static TSource Max<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => Handle(source, Enumerable.Max);
}

