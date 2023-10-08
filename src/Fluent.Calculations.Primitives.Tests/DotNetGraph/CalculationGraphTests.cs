using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.DotNetGraph
{
    public class CalculationGraphTests
    {
        [Fact]
        public async Task CalculationGraphRenderer_Should_Render_a_File()
        {
            Number result = new FooBarCalculation
            {
                ChildDeduction = Number.Of(200),
                NumberOfChildren = Number.Of(3),
                MinimumNumberOfChildren = Number.Of(2),
                GrossSalary = Number.Of(1000)
            }
            .ToResult();

            var graphFileName = "graph4.dot";

            await new CalculationGraphRenderer(graphFileName).Render(result);

            File.Exists(graphFileName).Should().BeTrue();
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