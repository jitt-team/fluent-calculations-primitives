namespace Fluent.Calculations.Primitives.BaseTypes;

using System;
using static Expressions.MathExpressionHandler;

public static class NumberMath
{
    public static T Abs<T>(T value) where T : IValue, new() => HandleWithSingleArgument<T>(value, Math.Abs);

    public static T Ceiling<T>(T value) where T : IValue, new() => HandleWithSingleArgument<T>(value, Math.Ceiling);

    public static T Floor<T>(T value) where T : IValue, new() => HandleWithSingleArgument<T>(value, Math.Floor);

    public static T Truncate<T>(T value) where T : IValue, new() => HandleWithSingleArgument<T>(value, Math.Truncate);

    public static T Min<T>(T left, T right) where T : IValue, new() => HandleWithTwoArguments<T>(left, right, Math.Min);

    public static T Max<T>(T left, T right) where T : IValue, new() => HandleWithTwoArguments<T>(left, right, Math.Max);

    public static T Round<T>(T left, T right) where T : IValue, new() => HandleWithTwoArguments<T>(left, right, (a, b) => Math.Round(a, Convert.ToInt32(b)));
}
