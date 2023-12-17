using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.EndToEnd
{
    public class NestedCalculationsTests
    {
        [Fact]
        public void Calculation_CallToOtherCalculation_WithArgument()
        {
            var scope = this.GetScope();

            Number
                one = Number.Of(1),
                two = Number.Of(2),
                three = Number.Of(3),
                four = Number.Of(4);

            Number result = scope.Evaluate(() => one + two * three - four * three + OtherCalculation(two));

            result.Expression.Arguments.Count.Should().Be(5);
        }

        private Number OtherCalculation(Number input)
        {
            Number
                three = Number.Of(3, nameof(three)),
                four = Number.Of(4, nameof(four));

            return this.GetScope().Evaluate(() => four * three + input);
        }
    }
}