namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public static class ValuesLinqMathExtensions
{
    public static TSource Sum<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Sum);

    public static TSource Average<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Average);

    public static TSource Min<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Min);

    public static TSource Max<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Max);
}