using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Integration
{
    public class ConstantsComparisionAndConditionalTests
    {
        [Fact]
        public void ConstantsComparisionAndConditional_IsExpectedResult()
        {
            var calculation = new ConstantsComparisionAndConditional
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.ToResult();

            result.Should().NotBeNull();
        }
    }

    internal class ConstantsComparisionAndConditional : EvaluationContext<Number>
    {
        public ConstantsComparisionAndConditional() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

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
