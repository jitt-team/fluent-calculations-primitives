using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Integration
{

    public class ConstantsAndNestedEvaluationTests
    {
        [Fact]
        public void ConstantsAndNestedEvaluation_IsExpectedResult()
        {
            ConstantsAndNestedEvaluations calculation = new()
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3),
                ConstantThree = Number.Of(4)
            };

            Number result = calculation.ToResult();

            result.Should().NotBeNull();
        }
    }

    internal class ConstantsAndNestedEvaluations : EvaluationScope<Number>
    {
        public ConstantsAndNestedEvaluations() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero,
            ConstantThree = Number.Zero;

        public Condition
            MyCondition = Condition.True();

        Number EvaluationOne => Evaluate(() => ConstantOne + ConstantTwo);

        Number EvaluationTwo => Evaluate(() => EvaluationOne * ConstantThree);

        public override Number Return() => EvaluationTwo;
    }
}
