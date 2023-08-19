using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives;
using FluentAssertions;

namespace Fluent.Calculations.Tests.Calculation
{
    public class CalculationTests2
    {
        [Fact]
        public async Task Test()
        {
            var calculation = new FooBarCalculation2
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.Calculate();

            await new CalculationGraphRenderer().Render(result);

            result.Should().NotBeNull();
        }
    }

    internal class FooBarCalculation2 : Calculation<Number>
    {
        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero;

        public Condition
            FeatureOn = Condition.True();

        Number Final => Is(() => FeatureOn ? ConstantOne + ConstantTwo : ConstantTwo);

        public override Number Return() => Final;
    }
}
