using Fluent.Calculations.DotNetGraph;

namespace Fluent.Calculations.Primitives.Tests.Graph
{
    public class CalculationGraphTests
    {
        [Fact]
        public async Task Test()
        {
            Number result = new FooBarCalculation
            {
                ChildDeduction = Number.Of(200),
                NumberOfChildren = Number.Of(3),
                MinimumNumberOfChildren = Number.Of(2),
                GrossSalary = Number.Of(1000)
            }
            .Calculate();

            await new CalculationGraphRenderer().Render(result);
        }

        internal class FooBarCalculation : EvaluationContext<Number>
        {
            public Number
                ChildDeduction = Number.Zero,
                NumberOfChildren = Number.Zero,
                MinimumNumberOfChildren = Number.Zero,
                GrossSalary = Number.Zero;

            Condition HasEnoughChildren => Evaluate(() => NumberOfChildren > MinimumNumberOfChildren);

            Number TotalChildrenDeduction => Evaluate(() => ChildDeduction * NumberOfChildren);

            Number AppliedChildDeduction => Evaluate(() => HasEnoughChildren ? TotalChildrenDeduction : Number.Zero);

            Number TaxableSalary => Evaluate(() => GrossSalary - AppliedChildDeduction);

            public override Number Return() => TaxableSalary;
        }
    }
}