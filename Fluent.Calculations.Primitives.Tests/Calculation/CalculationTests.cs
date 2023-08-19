using Fluent.Calculations.Primitives;
using FluentAssertions;

namespace Fluent.Calculations.Tests.Calculation
{
    public class CalculationTests
    {
        [Fact]
        public void Test()
        {
            var calculation = new FooBarCalculation
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.Calculate();

            result.Should().NotBeNull();
        }
    }

    internal class FooBarCalculation : Calculation<Number>
    {
        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero;

        public Condition
            FeatureOn = Condition.True();

        Condition Comparison => Is(() => ConstantOne > ConstantTwo);

        Condition FinalDecision => Is(() => FeatureOn && Comparison);

        Number Condtitional => Is(() => FinalDecision ? ConstantOne + ConstantOne : ConstantTwo);

        Number Math => Is(() => ConstantOne + Condtitional);

        public override Number Return() => Math;
    }
}
