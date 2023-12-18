namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;

public class MoneyBuilder
{
    private readonly Number value;

    public MoneyBuilder(Number value)
    {
        this.value = value;
    }

    public Money EUR => new(value, new Currency("EUR"));
}