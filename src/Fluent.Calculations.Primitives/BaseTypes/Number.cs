﻿namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

public class Number : Value,
    IAdditionOperators<Number, Number, Number>,
    ISubtractionOperators<Number, Number, Number>,
    IMultiplyOperators<Number, Number, Number>,
    IDivisionOperators<Number, Number, Number>,
    IComparisonOperators<Number, Number, Condition>,
    IEqualityOperators<Number, Number, Condition>
{
    public override string ToString() => $"{Name}";

    public Number() : this(CreateValueArgs.Create("Zero", ExpressionNodeConstant.Create($"0"), 0)) { }

    public Number(Number number) : base(number)
    {
    }

    public Number(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public static Number Zero => new();

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") =>
        new(CreateValueArgs.Create(fieldName, ExpressionNodeConstant.Create($"{primitiveValue}"), primitiveValue));

    public static Number operator -(Number left, Number right) => left.Substract(right);

    public static Number operator +(Number left, Number right) => left.Add(right);

    public static Number operator /(Number left, Number right) => left.Divide(right);

    public static Number operator *(Number left, Number right) => left.Multiply(right);

    public static Condition operator >(Number left, Number right) => left.GreaterThan(right);

    public static Condition operator <(Number left, Number right) => left.LessThan(right);

    public static Condition operator >=(Number left, Number right) => left.GreaterThanOrEqual(right);

    public static Condition operator <=(Number left, Number right) => left.LessThanOrEqual(right);

    public static Condition operator ==(Number? left, Number? right) => Enforce.NotNull(left).IsEqual(Enforce.NotNull(right));

    public static Condition operator !=(Number? left, Number? right) => Enforce.NotNull(left).NotEqual(Enforce.NotNull(right));

    private Condition IsEqual(Number right) => HandleConditionOperation(right, (a, b) => a == b);

    private Condition NotEqual(Number right) => HandleConditionOperation(right, (a, b) => a != b);

    private Condition LessThan(Number right) => HandleConditionOperation(right, (a, b) => a < b);

    private Condition GreaterThan(Number right) => HandleConditionOperation(right, (a, b) => a > b);

    private Condition LessThanOrEqual(Number right) => HandleConditionOperation(right, (a, b) => a <= b);

    private Condition GreaterThanOrEqual(Number right) => HandleConditionOperation(right, (a, b) => a >= b);

    public Number Add(Number right) => HandleNumberOperation(right, (a, b) => a + b);

    public Number Substract(Number right) => HandleNumberOperation(right, (a, b) => a - b);

    public Number Multiply(Number right) => HandleNumberOperation(right, (a, b) => a * b);

    public Number Divide(Number right) => HandleNumberOperation(right, (a, b) => a / b);

    private Condition HandleConditionOperation(IValue value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = "") =>
        HandleBinaryExpression<Condition, bool>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    private Number HandleNumberOperation(IValue value, Func<decimal, decimal, decimal> compareFunc, [CallerMemberName] string operatorName = "") =>
        HandleBinaryExpression<Number, decimal>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    public override IValue Create(CreateValueArgs args) => new Number(args);

    public override IValue Default => Zero;

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => base.GetHashCode();
}