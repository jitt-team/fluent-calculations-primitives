using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Integration
{
    public class ConstantsAndConditionsTests
    {
        [Fact]
        public void ConstantsAndConditions_IsExpectedResult()
        {
            ConstantsAndConditions calculation = new()
            {
                ConstantOne = Number.Of(1),
                ConstantTwo = Number.Of(2),
                ConstantThree = Number.Of(2)
            };

            Number result = calculation.ToResult();

            result.Should().NotBeNull();
        }
    }

    internal class ConstantsAndConditions : EvaluationContext<Number>
    {
        public ConstantsAndConditions() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        public Number
            ConstantOne = Number.Of(1),
            ConstantTwo = Number.Of(2),
            ConstantThree = Number.Of(3);

        public Condition
            MyCondition = Condition.True(),
            MyCondtitionTwo = Condition.False();

        Condition ConditionAnd => Evaluate(() => MyCondition && MyCondtitionTwo);

        Number MyResult => Evaluate(() => ConditionAnd ? ConstantOne : ConstantOne + ConstantThree);

        public override Number Return() => MyResult;
    }
}
