using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives.Tests.Composition;

public static class ToMoney
{
    public static MoneyBuilder Amount(this Number value, [CallerMemberName] string expressionName = "") => new MoneyBuilder(value, expressionName);
}