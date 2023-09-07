using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Calculation
{
    public class CalculationTestsThree
    {
        [Fact]
        public async Task Test()
        {
            var calculation = new MyCalculation3
            {
                ConstantOne = Number.Of(7),
                ConstantTwo = Number.Of(6),
                ConstantThree = Number.Of(5)
            };

            Number result = calculation.ToResult();

            await new CalculationGraphRenderer("graph3.dot").Render(result);

            result.Should().NotBeNull();
        }
    }

    internal class MyCalculation3 : EvaluationContext<Number>
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
