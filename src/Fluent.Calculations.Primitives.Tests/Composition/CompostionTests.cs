using Fluent.Calculations.Finance;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Inheritance
{
    public class CompostionTests
    {
        [Fact]
        public void CustomClass()
        {
            Money result = new MoneyCalc().GetResult();
            result.Should().NotBeNull();
        }

        [Fact]
        public void InheritedClass()
        {
            Money result = new MoneyCalcInherited().Calculate();
            result.Should().NotBeNull();
        }

        [Fact]
        public void SimpleMethod()
        {
            Money result = MoneyCalcMethod();
            result.Should().NotBeNull();
        }

        [Fact]
        public void SimpleFunction()
        {
            Money result = MoneyCalcFunction();
            result.Should().NotBeNull();
        }


        [Fact]
        public void SimpleFunctionMixedScopes()
        {
            Money result = MoneyCalcMethodMixedScopes();
            result.Should().NotBeNull();
        }

        private Money MoneyCalcMethod()
        {
            Scope<Money> Calculation = new Scope<Money>();

            Money
                MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
                MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR;

            return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
        }

        private Money FunctionToScopeImpicit()
        {
            Scope<Money> Calculation = new Scope<Money>();

            Money
                MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
                MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR;

            return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
        }

        private Money MoneyCalcMethodMixedScopes()
        {
            Scope<Money> Calculation1 = new Scope<Money>();
            Scope<Money> Calculation2 = new Scope<Money>();

            Money
                MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
                MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR,
                MoneyThree = ToMoney.Amount(Number.Of(10)).EUR,
                MoneyFour = ToMoney.Amount(Number.Of(10)).EUR;

            Money Calc1 = Calculation1.Evaluate(() => MoneyOne + MoneyTwo);
            Money Calc2 = Calculation2.Evaluate(() => MoneyThree + MoneyFour);

            return Calculation2.Evaluate(() => Calc1 + Calc2);
        }

        private Money MoneyCalcFunction()
        {
            Scope<Money> Calculation = new Scope<Money>(calculation =>
            {
                Money
                     MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
                     MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR;

                return calculation.Evaluate(() => MoneyOne + MoneyTwo);
            });

            return Calculation.Calculate();
        }
    }

    public class MoneyCalc
    {
        private Scope<Money> Calculation = new Scope<Money>();

        Money
            MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
            MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR;

        public Money GetResult() => Calculation.Evaluate(() => MoneyOne + MoneyTwo);
    }

    public class MoneyCalcInherited : Scope<Money>
    {
        Money
            MoneyOne = ToMoney.Amount(Number.Of(10)).EUR,
            MoneyTwo = ToMoney.Amount(Number.Of(20)).EUR;

        public override Money Return() => Evaluate(() => MoneyOne + MoneyTwo);
    }
}
