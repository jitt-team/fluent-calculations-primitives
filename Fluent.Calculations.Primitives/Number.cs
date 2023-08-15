using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Number : Value
{
    // TODO: Explore new static member interfaces for operator overloads
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/static-abstracts-in-interfaces#interfaces-as-type-arguments
    public static Number Zero => Number.Of(0, "Zero");

    public static Number operator +(Number a, Number b) => a.Add(a, b);

    public static Number From(decimal value, [CallerMemberName] string expressionName = "") => Number.Of(value, expressionName);

    public Number(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public Number Add(Number a, Number b) => new Number(CreateValueArgs.Compose(
        "Math:Add",
        $"{a}+{b}",
         a.PrimitiveValue + b.PrimitiveValue)
        .WithArguments(a, b));

    public Number Divide(Number a, Number b) => new Number(CreateValueArgs.Compose(
        "Math:Divide",
        $"{a}/{b}",
        a.PrimitiveValue / b.PrimitiveValue)
        .WithArguments(a, b));

    public static Number Of(decimal primitiveValue, [CallerMemberName] string fieldName = "") => new Number(CreateValueArgs.Compose(
        fieldName,
        $"{primitiveValue}",
        primitiveValue));


    public static Condition operator >(Number a, Number b) => new Condition(CreateValueArgs.Compose(
        "Logic:GreaterThan",
        $"{a}>{b}",
         Convert.ToDecimal(a.PrimitiveValue > b.PrimitiveValue))
        .WithArguments(a, b));

    public static Condition operator <(Number a, Number b) => new Condition(CreateValueArgs.Compose(
        "Logic:LessThan",
        $"{a}>{b}",
         Convert.ToDecimal(a.PrimitiveValue < b.PrimitiveValue))
        .WithArguments(a, b));

    public override IValue ToExpressionResult(IValue expressionResult, string expressionName, string expressionBody) => new Number(CreateValueArgs.Compose(
        expressionName,
        expressionBody,
        expressionResult.PrimitiveValue)
        .WithArguments(expressionResult.Arguments));
}