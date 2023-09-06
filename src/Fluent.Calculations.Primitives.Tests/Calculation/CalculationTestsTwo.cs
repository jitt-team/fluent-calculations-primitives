using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Calculation
{
    public class CalculationTestsTwo
    {
        [Fact]
        public async Task Test()
        {
            var calculation = new MyCalculation
            {
                ConstantOne = Number.Of(1),
                ConstantTwo = Number.Of(2),
                ConstantThree = Number.Of(2)
            };

            Number result = calculation.ToResult();

            await new CalculationGraphRenderer("graph2.dot").Render(result);

            result.Should().NotBeNull();
        }
    }

    internal class MyCalculation : EvaluationContext<Number>
    {
        public Number
            ConstantOne = Number.Of(1),
            ConstantTwo = Number.Of(2),
            ConstantThree = Number.Of(3);

        public Condition
            MyCondition = Condition.True();

        Number MyResultOne => Evaluate(() => ConstantTwo + ConstantThree);

        Number MyResult => Evaluate(() => ConstantOne + ConstantTwo + ConstantThree + MyResultOne);

        public override Number Return() => MyResult;
    }
}
