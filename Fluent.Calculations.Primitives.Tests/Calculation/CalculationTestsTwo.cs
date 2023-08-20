using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives;
using FluentAssertions;

namespace Fluent.Calculations.Tests.Calculation
{
    public class CalculationTestsTwo
    {
        [Fact]
        public async Task Test()
        {
            var calculation = new MyCalculation
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3)
            };

            Number result = calculation.Calculate();

            await new CalculationGraphRenderer().Render(result);

            result.Should().NotBeNull();
        }
    }

    internal class MyCalculation : Calculation<Number>
    {
        public Number
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3);

        public Condition
            MyCondition = Condition.True();

        Number MyResult => Is(() => MyCondition ? ConstantOne : ConstantTwo);

        public override Number Return() => MyResult;
    }
}
