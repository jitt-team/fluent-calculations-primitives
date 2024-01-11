namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Diagnostics;
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

    /// <include file="Docs.xml" path='*/Number/op_Subtraction/*'/>
    public static Number operator -(Number left, Number right) => left.Subtraction(right);

    /// <include file="Docs.xml" path='*/Number/op_Addition/*'/>
    public static Number operator +(Number left, Number right) => left.Addition(right);

    /// <include file="Docs.xml" path='*/Number/op_Division/*'/>
    public static Number operator /(Number left, Number right) => left.Division(right);

    /// <include file="Docs.xml" path='*/Number/op_Multiply/*'/>
    public static Number operator *(Number left, Number right) => left.Multiply(right);

    /// <include file="Docs.xml" path='*/Number/op_LessThan/*'/>
    public static Condition operator <(Number left, Number right) => left.LessThan(right);

    /// <include file="Docs.xml" path='*/Number/op_LessThanOrEqual/*'/>
    public static Condition operator <=(Number left, Number right) => left.LessThanOrEqual(right);

    /// <include file="Docs.xml" path='*/Number/op_GreaterThan/*'/>
    public static Condition operator >(Number left, Number right) => left.GreaterThan(right);

    /// <include file="Docs.xml" path='*/Number/op_GreaterThanOrEqual/*'/>
    public static Condition operator >=(Number left, Number right) => left.GreaterThanOrEqual(right);

    /// <include file="Docs.xml" path='*/Number/op_Equality/*'/>    
    public static Condition operator ==(Number? left, Number? right) => Enforce.NotNull(left).Equality(Enforce.NotNull(right));

    /// <include file="Docs.xml" path='*/Number/op_Inequality/*'/>
    public static Condition operator !=(Number? left, Number? right) => Enforce.NotNull(left).Inequality(Enforce.NotNull(right));

    /// <include file="Docs.xml" path='*/Number/generic_op_GreaterThan/*'/>
    static bool IComparisonOperators<Number, Number, bool>.operator >(Number left, Number right) => left.Primitive > right.Primitive;

    /// <include file="Docs.xml" path='*/Number/generic_op_GreaterThanOrEqual/*'/>
    static bool IComparisonOperators<Number, Number, bool>.operator >=(Number left, Number right) => left.Primitive >= right.Primitive;

    /// <include file="Docs.xml" path='*/Number/generic_op_LessThan/*'/>
    static bool IComparisonOperators<Number, Number, bool>.operator <(Number left, Number right) => left.Primitive < right.Primitive;

    /// <include file="Docs.xml" path='*/Number/generic_op_LessThanOrEqual/*'/>
    static bool IComparisonOperators<Number, Number, bool>.operator <=(Number left, Number right) => left.Primitive <= right.Primitive;

    /// <include file="Docs.xml" path='*/Number/generic_op_Equality/*'/>
    static bool IEqualityOperators<Number, Number, bool>.operator ==(Number? left, Number? right) => Enforce.NotNull(left).Equality(Enforce.NotNull(right)).IsTrue;

    /// <include file="Docs.xml" path='*/Number/generic_op_Inequality/*'/>
    static bool IEqualityOperators<Number, Number, bool>.operator !=(Number? left, Number? right) => Enforce.NotNull(left).Inequality(Enforce.NotNull(right)).IsTrue;

    /// <include file="Docs.xml" path='*/Number/MakeOfThisType/*'/>
    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Number(args);

    /// <include file="Docs.xml" path='*/Number/MakeDefault/*'/>
    public override IValueProvider MakeDefault() => Zero;

    /// <include file="Docs.xml" path='*/Number/Equals/*'/>
    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    /// <include file="Docs.xml" path='*/Number/GetHashCode/*'/>
    public override int GetHashCode() => base.GetHashCode();

    /// <include file="Docs.xml" path='*/Number/Addition/*'/>
    protected Number Addition(Number right) => HandleArithmeticOperation(right, (a, b) => a + b);

    /// <include file="Docs.xml" path='*/Number/Subtraction/*'/>
    protected Number Subtraction(Number right) => HandleArithmeticOperation(right, (a, b) => a - b);

    /// <include file="Docs.xml" path='*/Number/Multiply/*'/>
    protected Number Multiply(Number right) => HandleArithmeticOperation(right, (a, b) => a * b);

    /// <include file="Docs.xml" path='*/Number/Division/*'/>
    protected Number Division(Number right) => HandleArithmeticOperation(right, (a, b) => a / b);

    /// <include file="Docs.xml" path='*/Number/Equality/*'/>
    protected Condition Equality(Number right) => HandleComparisonOperation(right, (a, b) => a == b);

    /// <include file="Docs.xml" path='*/Number/Inequality/*'/>
    protected Condition Inequality(Number right) => HandleComparisonOperation(right, (a, b) => a != b);

    /// <include file="Docs.xml" path='*/Number/LessThan/*'/>
    protected Condition LessThan(Number right) => HandleComparisonOperation(right, (a, b) => a < b);

    /// <include file="Docs.xml" path='*/Number/GreaterThan/*'/>
    protected Condition GreaterThan(Number right) => HandleComparisonOperation(right, (a, b) => a > b);

    /// <include file="Docs.xml" path='*/Number/LessThanOrEqual/*'/>
    protected Condition LessThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a <= b);

    /// <include file="Docs.xml" path='*/Number/GreaterThanOrEqual/*'/>
    protected Condition GreaterThanOrEqual(Number right) => HandleComparisonOperation(right, (a, b) => a >= b);

    private Condition HandleComparisonOperation(IValueProvider value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);

    private Number HandleArithmeticOperation(IValueProvider value, Func<decimal, decimal, decimal> calculationFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Number, decimal>(value, (a, b) => calculationFunc(a.Primitive, b.Primitive), operatorName);
}