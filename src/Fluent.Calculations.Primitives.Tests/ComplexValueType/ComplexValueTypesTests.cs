using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;

public class ComplexValueTypesTests
{
    [Fact]
    public void Addition_OfTwoDerivedValues_CorrectResult()
    {
        Money result = Return();
        result.Currency.Code.Should().Be("EUR");
        result.Primitive.Should().Be(30);
    }

    private Money Return()
    {
        var scope = Scope.CreateHere(this);

        Money
            MoneyOne = Number.Of(10).AsMoney().EUR,
            MoneyTwo = Number.Of(20).AsMoney().EUR;

        return scope.Evaluate(() => MoneyOne + MoneyTwo);
    }
}
