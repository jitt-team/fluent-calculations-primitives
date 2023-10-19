using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Tests.Arrays
{

    public partial class ValueArrayTests
    {
        [Fact]
        public void Test()
        {


        }

        public static class Constant
        {
            public const string TestEvaluationName = "TEST-ARRAY-EVALUAITON-NAME";
        }

        public class TestArrayCalcuation
        {
            public static Number Return()
            {
                EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
                EvaluationContext<Number> Calculation = new(options);

                Number
                    NumberOne = Number.Of(2),
                    NumberTwo = Number.Of(3);

                Numbers MultipleNumbers = new() { NumberOne, NumberTwo };

                return Calculation.Evaluate(() => MultipleNumbers.Sum(), Constant.TestEvaluationName);
            }

        }
    }
}