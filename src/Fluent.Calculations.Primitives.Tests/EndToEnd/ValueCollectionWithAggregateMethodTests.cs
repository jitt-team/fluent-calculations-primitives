using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.EndToEnd
{
    public static class Constant
    {
        public const string TestEvaluationName = "TEST-ARRAY-EVALUATION-NAME";
    }

    public class ValueCollectionWithAggregateMethodTests
    {
        [Fact]
        public void Calculation_WithCollectionAndAggregateMethod_IsExpectedResult()
        {
            var result = TestArrayCalcuation.Return();
            result.Should().NotBeNull();
            result.Primitive.Should().Be(5);
        }

        [Fact]
        public void Calculation_WithCollectionAndAggregateMethod_IsExpectedResult_2()
        {
            CollectionsEvaluations calculation = new();

            Number result = calculation.ToResult();
            result.Should().NotBeNull();
        }


        public class TestArrayCalcuation
        {
            public static Number Return()
            {
                EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
                EvaluationContext<Number> Calculation = new(options);

                Number
                    NumberOne = Number.Of(2, nameof(NumberOne)),
                    NumberTwo = Number.Of(3, nameof(NumberTwo));

                Values<Number> MultipleNumbers = Values<Number>.SumOf(() => new[] { NumberOne, NumberTwo }, nameof(MultipleNumbers));

                return Calculation.Evaluate(() => MultipleNumbers.Sum(), Constant.TestEvaluationName);
            }
        }

        internal class CollectionsEvaluations : EvaluationContext<Number>
        {
            public CollectionsEvaluations() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

            Number SingleNumber = Number.Of(2);

            Values<Number> MultipleNumbers = new()
            {
                Number.Of(2, "COLLECTION-NUMBER-1"),
                Number.Of(4, "COLLECTION-NUMBER-2"),
                Number.Of(6, "COLLECTION-NUMBER-3")
            };

            public Number AverageFromMultipleNumbers => Evaluate(() => MultipleNumbers.Average());

            Number EvaluationTwo => Evaluate(() => AverageFromMultipleNumbers * SingleNumber);

            public override Number Return() => EvaluationTwo;
        }
    }
}