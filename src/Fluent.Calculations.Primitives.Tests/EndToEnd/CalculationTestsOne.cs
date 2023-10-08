using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Integration
{
    public class CalculationTestsOne
    {
        [Fact]
        public async Task Test()
        {
            var calculation = new FooBarCalculation
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.ToResult();

            await new CalculationGraphRenderer("graph1.dot").Render(result);

            result.Should().NotBeNull();
        }
    }

    internal class FooBarCalculation : EvaluationContext<Number>
    {
        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero;

        public Condition
            FeatureOn = Condition.True();

        Condition Comparison => Evaluate(() => ConstantOne > ConstantTwo);

        Number Condtitional => Evaluate(() => Comparison ? ConstantOne : ConstantTwo);

        public override Number Return() => Condtitional;
    }
}
