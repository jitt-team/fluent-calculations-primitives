using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;

public class ComplexValueTypesTests
{
    private Money Return()
    {
        EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
        EvaluationContext<Money> Calculation = new EvaluationContext<Money>();

        Money
            MoneyOne = Number.Of(10).Amount().EUR,
            MoneyTwo = Number.Of(20).Amount().EUR;

        return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
    }
}
