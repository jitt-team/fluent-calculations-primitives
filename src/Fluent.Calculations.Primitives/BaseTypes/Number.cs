namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <include file="IntelliSense.xml" path='docs/members[@name="Number"]/class/*' />
public class Number : Value,
    IAdditionOperators<Number, Number, Number>,
    ISubtractionOperators<Number, Number, Number>,
    IMultiplyOperators<Number, Number, Number>,
    IDivisionOperators<Number, Number, Number>,
    IComparisonOperators<Number, Number, Condition>,
    IComparisonOperators<Number, Number, bool>
{
    public override string ToString() => $"{Name}";

    public Number() : this(MakeValueArgs.Compose(StringConstants.Zero, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Constant), 0)) { }

    public Number(Number number) : base(number) { }

    public Number(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }


    public static implicit operator Number(int primitiveValue) => Number.Of(primitiveValue);

    public static implicit operator Number(decimal primitiveValue) => Number.Of(primitiveValue);

    public static Number Zero => new() { Origin = ValueOriginType.Constant };

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = StringConstants.NaN) =>
        Of(primitiveValue, StringConstants.NaN, fieldName);

    public static Number Of(decimal primitiveValue, string scope, [CallerMemberName] string fieldName = StringConstants.NaN) =>
        new(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant), primitiveValue,
            ValueOriginType.Constant, scope));

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

    static bool IComparisonOperators<Number, Number, bool>.operator >(Number left, Number right) => left.Primitive > right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator >=(Number left, Number right) => left.Primitive >= right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator <(Number left, Number right) => left.Primitive < right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator <=(Number left, Number right) => left.Primitive <= right.Primitive;

    static bool IEqualityOperators<Number, Number, bool>.operator ==(Number? left, Number? right) => Enforce.NotNull(left).IsEqual(Enforce.NotNull(right)).IsTrue;

    static bool IEqualityOperators<Number, Number, bool>.operator !=(Number? left, Number? right) => Enforce.NotNull(left).NotEqual(Enforce.NotNull(right)).IsTrue;

    private Condition IsEqual(Number right) => HandleComparisonOperation(right, (a, b) => a == b);

    private Condition NotEqual(Number right) => HandleComparisonOperation(right, (a, b) => a != b);

    private Condition LessThan(Number right) => HandleComparisonOperation(right, (a, b) => a < b);

    private Condition GreaterThan(Number right) => HandleComparisonOperation(right, (a, b) => a > b);

    private Condition LessThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a <= b);

    private Condition GreaterThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a >= b);

    protected Number Add(Number right) => HandleArithmeticOperation(right, (a, b) => a + b);

    protected Number Substract(Number right) => HandleArithmeticOperation(right, (a, b) => a - b);

    protected Number Multiply(Number right) => HandleArithmeticOperation(right, (a, b) => a * b);

    protected Number Divide(Number right) => HandleArithmeticOperation(right, (a, b) => a / b);

    private Condition HandleComparisonOperation(IValueProvider value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);

    private Number HandleArithmeticOperation(IValueProvider value, Func<decimal, decimal, decimal> calculationFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Number, decimal>(value, (a, b) => calculationFunc(a.Primitive, b.Primitive), operatorName);

    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Number(args);

    public override IValueProvider MakeDefault() => Zero;

    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    public override int GetHashCode() => base.GetHashCode();
}