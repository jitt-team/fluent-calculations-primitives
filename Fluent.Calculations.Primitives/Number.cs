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

    public static Condition operator >(Number left, Number right) => new Condition(CreateValueArgs.Compose(
        "GreaterThan", ExpressionNodeComparison.Create($"{left} > {right}").WithArguments(left, right),
         Convert.ToDecimal(left.PrimitiveValue > right.PrimitiveValue)));

    public static Condition operator <(Number left, Number right) => new Condition(CreateValueArgs.Compose(
        "LessThan", ExpressionNodeComparison.Create($"{left} > {right}").WithArguments(left, right),
         Convert.ToDecimal(left.PrimitiveValue < right.PrimitiveValue)));

    public Number Add(Number value) => Return(value, "Add",
        ExpressionNodeMath.Create($"{this} + {value}").WithArguments(this, value), (a, b) => a + b);

    public Number Substract(Number value) => Return(value, "Substract",
        ExpressionNodeMath.Create($"{this} - {value}").WithArguments(this, value), (a, b) => a - b);

    public Number Multiply(Number value) => Return(value, "Multiply",
        ExpressionNodeMath.Create($"{this} * {value}").WithArguments(this, value), (a, b) => a * b);

    public Number Divide(Number value) => Return(value, "Divide",
        ExpressionNodeMath.Create($"{this} / {value}").WithArguments(this, value), (a, b) => a / b);

    public TValue Return<TValue>(TValue value, string operatorName, ExpressionNode operationNode, Func<decimal, decimal, decimal> calcFunc)
        where TValue : IValue => (TValue)value.ToExpressionResult(CreateValueArgs
            .Compose(operatorName, operationNode, calcFunc(this.PrimitiveValue, value.PrimitiveValue)));

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") => new Number(CreateValueArgs.Compose(
        fieldName, ExpressionNodeConstant.Create($"{primitiveValue}"), primitiveValue));

    public override IValue ToExpressionResult(CreateValueArgs args) => new Number(args);
}