namespace Fluent.Calculations.Primitives.BaseTypes;
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

    public Number() : this(MakeValueArgs.Compose(Constants.Zero, new ExpressionNode("0", ExpressionNodeType.Constant), 0)) { }

    public Number(Number number) : base(number) { }

    public Number(MakeValueArgs createValueArgs) : base(createValueArgs) { }

    public static implicit operator Number(int primitiveValue) => Number.Of(primitiveValue);

    public static implicit operator Number(decimal primitiveValue) => Number.Of(primitiveValue);

    public static Number Zero => new() { IsParameter = true };

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") =>
        new(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant), primitiveValue));

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

    private Condition HandleConditionOperation(IValue value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = Constants.NaN) =>
        HandleBinaryExpression<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);

    private Number HandleNumberOperation(IValue value, Func<decimal, decimal, decimal> compareFunc, [CallerMemberName] string operatorName = Constants.NaN) =>
        HandleBinaryExpression<Number, decimal>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);

    public override IValue MakeOfThisType(MakeValueArgs args) => new Number(args);

    public override IValue Default => Zero;

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => base.GetHashCode();
}