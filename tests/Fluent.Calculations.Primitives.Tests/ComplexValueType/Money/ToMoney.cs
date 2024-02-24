namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;

public static class ToMoney
{
    public static MoneyBuilder AsMoney(this Number value) => new(value);
}