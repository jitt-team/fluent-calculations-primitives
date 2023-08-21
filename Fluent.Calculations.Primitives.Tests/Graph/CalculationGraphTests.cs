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

        internal class FooBarCalculation : Calculation<Number>
        {
            public Number
                ChildDeduction = Number.Zero,
                NumberOfChildren = Number.Zero,
                MinimumNumberOfChildren = Number.Zero,
                GrossSalary = Number.Zero;

            Condition HasEnoughChildren => Is(() => NumberOfChildren > MinimumNumberOfChildren);

            Number TotalChildrenDeduction => Is(() => ChildDeduction * NumberOfChildren);

            Number AppliedChildDeduction => Is(() => HasEnoughChildren ? TotalChildrenDeduction : Number.Zero);

            Number TaxableSalary => Is(() => GrossSalary - AppliedChildDeduction);

            public override Number Return() => TaxableSalary;
        }
    }
}