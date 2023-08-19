using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Number : Value
{
    public static Number Zero => Number.Of(0, "Zero");

    public static Number From(decimal value, [CallerMemberName] string expressionName = "") => Number.Of(value, expressionName);

    public Number(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public Number() : base("Zero", 0) { }

    public static Number operator -(Number left, Number right) => left.Substract(right);

    public static Number operator +(Number left, Number right) => left.Add(right);

    public static Number operator /(Number left, Number right) => left.Divide(right);

    public static Number operator *(Number left, Number right) => left.Multiply(right);

    public static Condition operator >(Number left, Number right) => left.GreaterThan(right);

    public static Condition operator <(Number left, Number right) => left.LessThan(right);

    public static Condition operator >=(Number left, Number right) => left.GreaterThanOrEqual(right);

    public static Condition operator <=(Number left, Number right) => left.LessThanOrEqual(right);

    public static Condition operator ==(Number left, Number right) => left.IsEqual(right);

    public static Condition operator !=(Number left, Number right) => left.NotEqual(right);

    private Condition IsEqual(Number value) => ReturnCondition(value, (a, b) => a == b);

    private Condition NotEqual(Number value) => ReturnCondition(value, (a, b) => a != b);

    private Condition LessThan(Number value) => ReturnCondition(value, (a, b) => a < b);

    private Condition GreaterThan(Number value) => ReturnCondition(value, (a, b) => a > b);

    private Condition LessThanOrEqual(Number value) => ReturnCondition(value, (a, b) => a <= b);

    private Condition GreaterThanOrEqual(Number value) => ReturnCondition(value, (a, b) => a >= b);

    public Number Add(Number value) => ReturnNumber(value, (a, b) => a + b);

    public Number Substract(Number value) => ReturnNumber(value, (a, b) => a - b);

    public Number Multiply(Number value) => ReturnNumber(value, (a, b) => a * b);

    public Number Divide(Number value) => ReturnNumber(value, (a, b) => a / b);

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") =>
        new Number(CreateValueArgs.Compose(fieldName, ExpressionNodeConstant.Create($"{primitiveValue}"), primitiveValue));

    private Condition ReturnCondition(IValue value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = "") =>
        Return<Condition, bool>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    private Number ReturnNumber(IValue value, Func<decimal, decimal, decimal> compareFunc, [CallerMemberName] string operatorName = "") =>
        Return<Number, decimal>(value, (a, b) => compareFunc(a.PrimitiveValue, b.PrimitiveValue), operatorName);

    public override IValue ToExpressionResult(CreateValueArgs args) => new Number(args);
}