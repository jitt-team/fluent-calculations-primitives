namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;

public static class ToMoney
{
    public static MoneyBuilder AsMoney(this Number value, [CallerMemberName] string expressionName = "") => new MoneyBuilder(value, expressionName);
}