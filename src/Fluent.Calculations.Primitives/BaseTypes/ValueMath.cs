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
    public static T Min<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArguments<T>(left, right, Math.Min);

    /// <include file="Docs.xml" path='*/ValueMath/Max/*'/>
    public static T Max<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArguments<T>(left, right, Math.Max);

    /// <include file="Docs.xml" path='*/ValueMath/Round/*'/>
    public static T Round<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArgumentsInt<T>(left, right, Math.Round);
}
