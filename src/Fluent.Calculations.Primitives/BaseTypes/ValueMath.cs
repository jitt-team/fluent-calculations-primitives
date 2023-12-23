namespace Fluent.Calculations.Primitives.BaseTypes;
using System;
using static Expressions.MathExpressionHandler;

/// <include file="IntelliSense.xml" path='docs/members[@name="ValueMath"]/class/*' />
public static class ValueMath
{
    public static T Abs<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Abs);

    public static T Ceiling<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Ceiling);

    public static T Floor<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Floor);

    public static T Truncate<T>(T value) where T : IValueProvider, new() => HandleWithSingleArgument<T>(value, Math.Truncate);

    public static T Min<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArguments<T>(left, right, Math.Min);

    public static T Max<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArguments<T>(left, right, Math.Max);

    public static T Round<T>(T left, T right) where T : IValueProvider, new() => HandleWithTwoArgumentsInt<T>(left, right, Math.Round);
}
