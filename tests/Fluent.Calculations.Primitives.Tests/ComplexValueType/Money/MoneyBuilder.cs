namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;

public class MoneyBuilder(Number value)
{
    private readonly Number value = value;

    public Money EUR => new(value, new Currency("EUR"));
}