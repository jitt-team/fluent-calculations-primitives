using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class ValueArgumentsSelectorTests
    {
        [Fact]
        public void EndToEnd()
        {
            Scope scope = new();
            var ctx = scope.CreateContext<Number>();

            Number
                one = Number.Of(1),
                two = Number.Of(2),
                three = Number.Of(3),
                four = Number.Of(4);
            
            // Number otherResult = ctx.Evaluate(() => four * three, nameof(otherResult));

            Number result = ctx.Evaluate(() => one + two * three - four * three + OtherCalculation());

            var selector = new ValueArgumentsSelector();
            var squshedResult = selector.SelectArguments(result);

            squshedResult.Length.Should().Be(5);
        }


        [Fact]
        public void Test()
        {
            var selector = new ValueArgumentsSelector();
            var testResult = GetTestResult();
            var arguments = selector.SelectArguments(testResult);

            arguments.Length.Should().Be(4);
        }

        private static Number GetTestResult()
        {
            Number
                one = Number.Of(1, nameof(one)),
                two = Number.Of(2, nameof(two)),
                three = Number.Of(3, nameof(three)),
                four = Number.Of(4, nameof(four));

            Number result = one + two * three - four * three + OtherCalculation();

            return result;
        }

        private static Number OtherCalculation()
        {
            Scope scope = new();
            var ctx = scope.CreateContext<Number>();

            Number
                one = Number.Of(1, nameof(one)),
                two = Number.Of(2, nameof(two)),
                three = Number.Of(3, nameof(three)),
                four = Number.Of(4, nameof(four));

            return ctx.Evaluate(() => four * three);
        }

    }
}
