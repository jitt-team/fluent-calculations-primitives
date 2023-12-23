namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/Number/class/*'/>
public class Number : Value,
    IAdditionOperators<Number, Number, Number>,
    ISubtractionOperators<Number, Number, Number>,
    IMultiplyOperators<Number, Number, Number>,
    IDivisionOperators<Number, Number, Number>,
    IComparisonOperators<Number, Number, Condition>,
    IComparisonOperators<Number, Number, bool>
{
    /// <include file="Docs.xml" path='*/Number/ToString/*'/>
    public override string ToString() => $"{Name}";

    /// <include file="Docs.xml" path='*/Number/ctor/*'/>
    public Number() : this(MakeValueArgs.Compose(StringConstants.Zero, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Constant), 0)) { }

    /// <include file="Docs.xml" path='*/Number/ctor-number/*'/>
    public Number(Number number) : base(number) { }

    /// <include file="Docs.xml" path='*/Number/ctor-makeValueArgs/*'/>
    public Number(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }

    /// <include file="Docs.xml" path='*/Number/implicit-int/*'/>
    public static implicit operator Number(int primitiveValue) => Number.Of(primitiveValue);

    /// <include file="Docs.xml" path='*/Number/implicit-decimal/*'/>
    public static implicit operator Number(decimal primitiveValue) => Number.Of(primitiveValue);

    /// <include file="Docs.xml" path='*/Number/Zero/*'/>
    public static Number Zero => new() { Origin = ValueOriginType.Constant };

    /// <include file="Docs.xml" path='*/Number/Of/*'/>
    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = StringConstants.NaN) =>
        Of(primitiveValue, StringConstants.NaN, fieldName);

    /// <include file="Docs.xml" path='*/Number/Of-scope/*'/>
    public static Number Of(decimal primitiveValue, string scope, [CallerMemberName] string fieldName = StringConstants.NaN) =>
        new(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant), primitiveValue,
            ValueOriginType.Constant, scope));

    /// <include file="Docs.xml" path='*/Number/operator-substract/*'/>
    public static Number operator -(Number left, Number right) => left.Substract(right);

    /// <include file="Docs.xml" path='*/Number/operator-add/*'/>
    public static Number operator +(Number left, Number right) => left.Add(right);

    /// <include file="Docs.xml" path='*/Number/operator-divide/*'/>
    public static Number operator /(Number left, Number right) => left.Divide(right);

    /// <include file="Docs.xml" path='*/Number/operator-multiply/*'/>
    public static Number operator *(Number left, Number right) => left.Multiply(right);

    /// <include file="Docs.xml" path='*/Number/operator-greater-than/*'/>
    public static Condition operator >(Number left, Number right) => left.GreaterThan(right);

    /// <include file="Docs.xml" path='*/Number/operator-less-than/*'/>
    public static Condition operator <(Number left, Number right) => left.LessThan(right);

    /// <include file="Docs.xml" path='*/Number/operator-greater-or-equal/*'/>
    public static Condition operator >=(Number left, Number right) => left.GreaterThanOrEqual(right);

    /// <include file="Docs.xml" path='*/Number/operator-less-or-equal/*'/>
    public static Condition operator <=(Number left, Number right) => left.LessThanOrEqual(right);

    /// <include file="Docs.xml" path='*/Number/operator-equal/*'/>
    public static Condition operator ==(Number? left, Number? right) => Enforce.NotNull(left).IsEqual(Enforce.NotNull(right));

    /// <include file="Docs.xml" path='*/Number/operator-not-equal/*'/>
    public static Condition operator !=(Number? left, Number? right) => Enforce.NotNull(left).NotEqual(Enforce.NotNull(right));

    static bool IComparisonOperators<Number, Number, bool>.operator >(Number left, Number right) => left.Primitive > right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator >=(Number left, Number right) => left.Primitive >= right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator <(Number left, Number right) => left.Primitive < right.Primitive;

    static bool IComparisonOperators<Number, Number, bool>.operator <=(Number left, Number right) => left.Primitive <= right.Primitive;

    static bool IEqualityOperators<Number, Number, bool>.operator ==(Number? left, Number? right) => Enforce.NotNull(left).IsEqual(Enforce.NotNull(right)).IsTrue;

    static bool IEqualityOperators<Number, Number, bool>.operator !=(Number? left, Number? right) => Enforce.NotNull(left).NotEqual(Enforce.NotNull(right)).IsTrue;

    /// <include file="Docs.xml" path='*/Number/MakeOfThisType/*'/>
    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Number(args);

    /// <include file="Docs.xml" path='*/Number/MakeDefault/*'/>
    public override IValueProvider MakeDefault() => Zero;

    /// <include file="Docs.xml" path='*/Number/Equals/*'/>
    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    /// <include file="Docs.xml" path='*/Number/GetHashCode/*'/>
    public override int GetHashCode() => base.GetHashCode();

    /// <include file="Docs.xml" path='*/Number/Add/*'/>
    protected Number Add(Number right) => HandleArithmeticOperation(right, (a, b) => a + b);

    /// <include file="Docs.xml" path='*/Number/Substract/*'/>
    protected Number Substract(Number right) => HandleArithmeticOperation(right, (a, b) => a - b);

    /// <include file="Docs.xml" path='*/Number/Multiply/*'/>
    protected Number Multiply(Number right) => HandleArithmeticOperation(right, (a, b) => a * b);

    /// <include file="Docs.xml" path='*/Number/Divide/*'/>
    protected Number Divide(Number right) => HandleArithmeticOperation(right, (a, b) => a / b);

    private Condition IsEqual(Number right) => HandleComparisonOperation(right, (a, b) => a == b);

    private Condition NotEqual(Number right) => HandleComparisonOperation(right, (a, b) => a != b);

    private Condition LessThan(Number right) => HandleComparisonOperation(right, (a, b) => a < b);

    private Condition GreaterThan(Number right) => HandleComparisonOperation(right, (a, b) => a > b);

    private Condition LessThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a <= b);

    private Condition GreaterThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a >= b);

    private Condition HandleComparisonOperation(IValueProvider value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);

    private Number HandleArithmeticOperation(IValueProvider value, Func<decimal, decimal, decimal> calculationFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Number, decimal>(value, (a, b) => calculationFunc(a.Primitive, b.Primitive), operatorName);
}