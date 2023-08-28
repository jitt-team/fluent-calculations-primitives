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

    public Number() : this(CreateValueArgs.Compose("Zero", ExpressionNodeConstant.Create($"0"), 0)) { }

    public Number(Number number) : base(number)
    {
    }

    public Number(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public static Number Zero => new Number();

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") =>
        new Number(CreateValueArgs.Compose(fieldName, ExpressionNodeConstant.Create($"{primitiveValue}"), primitiveValue));

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

    private Condition IsEqual(Number right) => ReturnCondition(right, (a, b) => a == b);

    private Condition NotEqual(Number right) => ReturnCondition(right, (a, b) => a != b);

    private Condition LessThan(Number right) => ReturnCondition(right, (a, b) => a < b);

    private Condition GreaterThan(Number right) => ReturnCondition(right, (a, b) => a > b);

    private Condition LessThanOrEqual(Number right) => ReturnCondition(right, (a, b) => a <= b);

    private Condition GreaterThanOrEqual(Number right) => ReturnCondition(right, (a, b) => a >= b);

    public Number Add(Number right) => ReturnNumber(right, (a, b) => a + b);

    public Number Substract(Number right) => ReturnNumber(right, (a, b) => a - b);

    public Number Multiply(Number right) => ReturnNumber(right, (a, b) => a * b);

    public Number Divide(Number right) => ReturnNumber(right, (a, b) => a / b);

    private Condition ReturnCondition(IValue value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = "") =>
        Return<Condition, bool>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    // TODO: Can we leverage this as a Generic Math aka to creare reusable component to compose IValue results but letting the ACTUAL logic open for inheriting classes?
    private Number ReturnNumber(IValue value, Func<decimal, decimal, decimal> compareFunc, [CallerMemberName] string operatorName = "") =>
        Return<Number, decimal>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    public override IValue Compose(CreateValueArgs args) => new Number(args);

    public override IValue Default => Zero;

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => base.GetHashCode();
}