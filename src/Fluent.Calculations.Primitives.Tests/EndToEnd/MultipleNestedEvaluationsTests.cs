using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Demo
{
    public class MultipleNestedEvaluationsTests
    {
        [Fact]
        public void NestedEvaluations_IsExpectedResult()
        {
            MultipleNestedEvaluations evaluation = new()
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(10),
                ConstantThree = Number.Of(5),
                ConstantFour = Number.Of(30)
            };

            Number result = evaluation.ToResult();

            result.Primitive.Should().Be(50);
            result.Name.Should().Be(nameof(evaluation.ConstantFourPlusWhenTrue));
        }

        internal class MultipleNestedEvaluations : EvaluationContext<Number>
        {
            public MultipleNestedEvaluations() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

            public Number
                ConstantOne = Number.Zero,
                ConstantTwo = Number.Zero,
                ConstantThree = Number.Zero,
                ConstantFour = Number.Zero;

            public Condition ConstantOneGreaterThanTwo => Evaluate(() => ConstantTwo > ConstantThree);
            public Number ConstantOneTimesTwo => Evaluate(() => ConstantOne * ConstantTwo);
            public Number WhenTrueThenValue => Evaluate(() => ConstantOneGreaterThanTwo ? ConstantOneTimesTwo : Number.Zero);
            public Number ConstantFourPlusWhenTrue => Evaluate(() => ConstantFour + WhenTrueThenValue);
            public override Number Return() => ConstantFourPlusWhenTrue;
        }
    }
}