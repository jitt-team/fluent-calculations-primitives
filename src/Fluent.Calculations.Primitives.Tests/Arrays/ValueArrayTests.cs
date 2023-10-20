using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Arrays
{
    public static class Constant
    {
        public const string TestEvaluationName = "TEST-ARRAY-EVALUAITON-NAME";
    }

    public partial class ValueArrayTests
    {
        [Fact]
        public void Test()
        {

            var result = TestArrayCalcuation.Return();
            result.Should().NotBeNull();
            result.Primitive.Should().Be(20);
        }

        public class TestArrayCalcuation
        {
            public static Number Return()
            {
                EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
                EvaluationContext<Number> Calculation = new(options);

                Number
                    NumberOne = Number.Of(2),
                    NumberTwo = Number.Of(3),
                    NumberThree = Number.Of(4);

                Numbers MultipleNumbers = Numbers.Of(() => new[] { NumberOne, NumberTwo });

                return Calculation.Evaluate(() => MultipleNumbers.Sum() * NumberThree, Constant.TestEvaluationName);
            }

        }
    }
}