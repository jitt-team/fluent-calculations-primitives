using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class ValueArgumentsSelectorTests
    {


        [Fact]
        public void Test()
        {
            var selector = new ValueArgumentsSelector();
            var testResult = GetPlainTestResult();
            var arguments = selector.Select(testResult);

            // arguments.Length.Should().Be(4);
        }

        private Number GetPlainTestResult()
        {
            Number
                one = Number.Of(1),
                two = Number.Of(2),
                three = Number.Of(3),
                four = Number.Of(4);

            return one + two * three - four * three + OtherCalculation(two);
        }

        private Number OtherCalculation(Number two)
        {
            Number
                a = Number.Of(1),
                b = Number.Of(2);

            return a + b;
        }
    }
}
