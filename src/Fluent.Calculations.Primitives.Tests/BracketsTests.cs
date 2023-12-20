using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Business.Tests
{
    public class BracketsTests
    {
        [Fact]
        public void Test()
        {
            var brackets = RateBrackets<Number, Number>
                .UpTo(10).Multiplier(0.02m)
                .UpTo(20).Multiplier(0.03m)
                .UpTo(30).Multiplier(0.10m)
                .ReminderMultiplier(0.15m);

            var result = brackets.Evaluate(100);

            result.Amount.Primitive.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestDecimal()
        {
            var brackets = RateBrackets<decimal, decimal>
                .UpTo(10).Multiplier(0.02m)
                .UpTo(20).Multiplier(0.03m)
                .UpTo(30).Multiplier(0.10m)
                .ReminderMultiplier(0.15m);

            var result = brackets.Evaluate(100);

            result.Amount.Should().BeGreaterThan(0);
        }
    }
}
