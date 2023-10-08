using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.DotNetGraph
{
    public class CalculationDotGraphRendererTests
    {
        [Fact]
        public async Task TestWIP()
        {
            var calculation = new FooBarCalculation
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.ToResult();

            result.Should().NotBeNull();

            var graphFileName = "graph4.dot";

            await new CalculationDotGraphRenderer(graphFileName).Render(result);

            File.Exists(graphFileName).Should().BeTrue();
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
