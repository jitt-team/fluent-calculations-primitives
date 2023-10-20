using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;

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

                return Calculation.Evaluate(() => MultipleNumbers.Sum() * NumberOne, Constant.TestEvaluationName);
            }

        }
    }
}