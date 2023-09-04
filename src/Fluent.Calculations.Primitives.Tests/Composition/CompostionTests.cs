using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Composition;

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
        Money result = new MoneyCalcInherited().ToResult();
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
        EvaluationContext<Money> Calculation = new EvaluationContext<Money>();

        Money
            MoneyOne = Number.Of(10).Amount().EUR,
            MoneyTwo = Number.Of(20).Amount().EUR;

        return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
    }

    private Money FunctionToScopeImpicit()
    {
        EvaluationContext<Money> Calculation = new EvaluationContext<Money>();

        Money
            MoneyOne = Number.Of(10).Amount().EUR,
            MoneyTwo = Number.Of(20).Amount().EUR;

        return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
    }

    private Money MoneyCalcMethodMixedScopes()
    {
        EvaluationContext<Money> Calculation1 = new EvaluationContext<Money>();
        EvaluationContext<Money> Calculation2 = new EvaluationContext<Money>();

        Money
            MoneyOne = Number.Of(10).Amount().EUR,
            MoneyTwo = Number.Of(20).Amount().EUR,
            MoneyThree = Number.Of(10).Amount().EUR,
            MoneyFour = Number.Of(10).Amount().EUR;

        Money Calc1 = Calculation1.Evaluate(() => MoneyOne + MoneyTwo);
        Money Calc2 = Calculation2.Evaluate(() => MoneyThree + MoneyFour);

        return Calculation2.Evaluate(() => Calc1 + Calc2);
    }

    private Money MoneyCalcFunction()
    {
        EvaluationContext<Money> Calculation = new EvaluationContext<Money>(calculation =>
        {
            Money
                 MoneyOne = Number.Of(10).Amount().EUR,
                 MoneyTwo = Number.Of(20).Amount().EUR;

            return calculation.Evaluate(() => MoneyOne + MoneyTwo);
        });

        return Calculation.ToResult();
    }
}

public class MoneyCalc
{
    private EvaluationContext<Money> Calculation = new EvaluationContext<Money>();

    Money
        MoneyOne = Number.Of(10).Amount().EUR,
        MoneyTwo = Number.Of(20).Amount().EUR;

    public Money GetResult() => Calculation.Evaluate(() => MoneyOne + MoneyTwo);
}

public class MoneyCalcInherited : EvaluationContext<Money>
{
    Money
        MoneyOne = Number.Of(10).Amount().EUR,
        MoneyTwo = Number.Of(20).Amount().EUR;

    public override Money Return() => Evaluate(() => MoneyOne + MoneyTwo);
}
