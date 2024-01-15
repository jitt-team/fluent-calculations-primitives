namespace Fluent.Calculations.Primitives.BaseTypes;
using System;
using static Expressions.MathExpressionHandler;

/// <include file="Docs.xml" path='*/ValueMath/class/*'/>
public static class ValueMath
{
    /// <include file="Docs.xml" path='*/ValueMath/Abs/*'/>
    public static T Abs<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Abs);

    /// <include file="Docs.xml" path='*/ValueMath/Ceiling/*'/>
    public static T Ceiling<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Ceiling);

    /// <include file="Docs.xml" path='*/ValueMath/Floor/*'/>
    public static T Floor<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Floor);

    /// <include file="Docs.xml" path='*/ValueMath/Truncate/*'/>
    public static T Truncate<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Truncate);

    /// <include file="Docs.xml" path='*/ValueMath/Min/*'/>
    public static T Min<T>(T val1, T val2) where T : IValueProvider, new() => HandleWithTwoArguments<T>(val1, val2, Math.Min);

    /// <include file="Docs.xml" path='*/ValueMath/Max/*'/>
    public static T Max<T>(T val1, T val2) where T : IValueProvider, new() => HandleWithTwoArguments<T>(val1, val2, Math.Max);

    /// <include file="Docs.xml" path='*/ValueMath/Round/*'/>
    public static T Round<T>(T d, T decimals) where T : IValueProvider, new() => HandleWithTwoArgumentsInt<T>(d, decimals, Math.Round);
}
