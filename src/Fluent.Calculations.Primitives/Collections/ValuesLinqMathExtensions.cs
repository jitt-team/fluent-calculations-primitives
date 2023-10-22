namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public static class ValuesLinqMathExtensions
{
    public static TSource Sum<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => source.Handle(Enumerable.Sum);

    public static TSource Average<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => source.Handle(Enumerable.Average);

    public static TSource Min<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => source.Handle(Enumerable.Min);

    public static TSource Max<TSource>(this IValues<TSource> source) where TSource : class, IValue, new() => source.Handle(Enumerable.Max);
}