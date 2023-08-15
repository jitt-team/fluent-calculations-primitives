using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Number : Value
{
    public static Number Zero => Number.Of(0, "Zero");

    public static Number From(decimal value, [CallerMemberName] string expressionName = "") => Number.Of(value, expressionName);

    public Number(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public Number(): base("Zero", 0) { }

    public static Number operator -(Number a, Number b) => a.Substract(b);

    public static Number operator +(Number a, Number b) => a.Add(b);

    public static Number operator /(Number a, Number b) => a.Divide(b);

    public static Number operator *(Number a, Number b) => a.Multiply(b);

    public static Condition operator >(Number a, Number b) => new Condition(CreateValueArgs.Compose(
        "GreaterThan", $"{a} > {b}",
         Convert.ToDecimal(a.PrimitiveValue > b.PrimitiveValue))
        .WithArguments(a, b));

    public static Condition operator <(Number a, Number b) => new Condition(CreateValueArgs.Compose(
        "LessThan", $"{a} > {b}",
         Convert.ToDecimal(a.PrimitiveValue < b.PrimitiveValue))
        .WithArguments(a, b));

    public Number Add(Number value) => Return(value, "Add", $"{this} + {value}", (a, b) => a + b);

    public Number Substract(Number value) => Return(value, "Substract", $"{this} - {value}", (a, b) => a - b);

    public Number Multiply(Number value) => Return(value, "Multiply", $"{this} * {value}", (a, b) => a * b);

    public Number Divide(Number value) => Return(value, "Divide", $"{this} / {value}", (a, b) => a / b);

    public TValue Return<TValue>(TValue value, string operatorName, string operationBody, Func<decimal, decimal, decimal> calcFunc)
        where TValue : IValue => (TValue)value.ToExpressionResult(CreateValueArgs
            .Compose(operatorName, operationBody, calcFunc(this.PrimitiveValue, value.PrimitiveValue))
            .WithArguments(this, value));

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") => new Number(CreateValueArgs.Compose(
        fieldName, $"{primitiveValue}", primitiveValue));

    public override IValue ToExpressionResult(CreateValueArgs args) => new Number(args);
}