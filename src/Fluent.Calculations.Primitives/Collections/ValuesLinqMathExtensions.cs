namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

/// <include file="Docs.xml" path='*/ValuesLinqMathExtensions/class/*'/>
public static class ValuesLinqMathExtensions
{
    /// <include file="Docs.xml" path='*/ValuesLinqMathExtensions/Sum/*'/>
    public static TSource Sum<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Sum);

    /// <include file="Docs.xml" path='*/ValuesLinqMathExtensions/Average/*'/>
    public static TSource Average<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Average);

    /// <include file="Docs.xml" path='*/ValuesLinqMathExtensions/Min/*'/>
    public static TSource Min<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Min);

    /// <include file="Docs.xml" path='*/ValuesLinqMathExtensions/Max/*'/>
    public static TSource Max<TSource>(this IValuesProvider<TSource> source) where TSource : class, IValueProvider, new() => source.Handle(Enumerable.Max);
}