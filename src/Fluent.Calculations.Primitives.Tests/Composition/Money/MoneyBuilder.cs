using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Tests.Composition;

public class MoneyBuilder
{
    private Number value;
    private string expressionName;

    public MoneyBuilder(Number value, string expressionName)
    {
        this.value = value;
        this.expressionName = expressionName;
    }

    public Money EUR => new Money(value, new Currency("EUR"));
}