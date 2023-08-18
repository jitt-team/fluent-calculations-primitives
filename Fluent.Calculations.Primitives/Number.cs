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

    public static Condition operator ==(Number left, Number right) => left.IsEqual(right);

    public static Condition operator !=(Number left, Number right) => left.NotEqual(right);

    private Condition IsEqual(Number value) => Return<Condition, bool>(value, "==", (a, b) => a == b);

    private Condition NotEqual(Number value) => Return<Condition, bool>(value, "!=", (a, b) => a != b);

    private Condition LessThan(Number value) => Return<Condition, bool>(value, "<", (a, b) => a < b);

    private Condition GreaterThan(Number value) => Return<Condition, bool>(value, ">", (a, b) => a > b);

    public Number Add(Number value) => Return<Number, decimal>(value, "+", (a, b) => a + b);

    public Number Substract(Number value) => Return<Number, decimal>(value, "-", (a, b) => a - b);

    public Number Multiply(Number value) => Return<Number, decimal>(value, "*", (a, b) => a * b);

    public Number Divide(Number value) => Return<Number, decimal>(value, "/", (a, b) => a / b);

    public ValueType Return<ValueType, PrimitiveType>(
        IValue right,
        string languageOperator,
        Func<decimal, decimal, PrimitiveType> calcFunc,
        [CallerMemberName] string operatorName = "")
        where ValueType : IValue, new()
    {
        ExpressionNode operationNode = ExpressionNodeMath.Create($"{this} {languageOperator} {right}").WithArguments(this, right);
        return (ValueType)new ValueType().ToExpressionResult(CreateValueArgs
            .Compose(operatorName, operationNode, Convert.ToDecimal(calcFunc(this.PrimitiveValue, right.PrimitiveValue))));
    }

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") => new Number(CreateValueArgs.Compose(
        fieldName, ExpressionNodeConstant.Create($"{primitiveValue}"), primitiveValue));

    public override IValue ToExpressionResult(CreateValueArgs args) => new Number(args);
}