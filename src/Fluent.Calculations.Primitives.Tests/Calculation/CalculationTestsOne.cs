using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Calculation
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

            await new CalculationGraphRenderer("graph3.dot").Render(result);

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

        Condition FinalDecision => Evaluate(() => FeatureOn && Comparison);

        Number Condtitional => Evaluate(() => FinalDecision ? ConstantOne + ConstantOne : ConstantTwo);

        Number MyMath => Evaluate(() => ConstantOne + Condtitional);

        public override Number Return() => MyMath;
    }
}
