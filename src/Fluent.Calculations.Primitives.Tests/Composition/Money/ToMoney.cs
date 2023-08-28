using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Finance;

public static class ToMoney
{
    public static MoneyBuilder Amount(this Number value, [CallerMemberName] string expressionName = "") => new MoneyBuilder(value, expressionName);
}