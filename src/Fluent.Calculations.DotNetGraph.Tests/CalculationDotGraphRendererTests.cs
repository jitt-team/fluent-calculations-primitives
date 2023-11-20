using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.DotNetGraph;

public class CalculationDotGraphRendererTests
{
    [Fact]
    public void AdditionCalculation_Rendered_GraphHasExpectedElements()
    {
        var calculation = new AdditionCalculation
        {
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3)
        };

        Number result = calculation.ToResult();

        DotGraph graph = new DotGraphValueBuilder().Build(result);

        graph.Elements.Should().HaveCount(6);
    }
}

internal class AdditionCalculation : EvaluationContext<Number>
{
    public AdditionCalculation() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

    public Number
        ConstantOne = Number.Zero,
        ConstantTwo = Number.Zero;

    Number OnePlusTwo => Evaluate(() => ConstantOne + ConstantTwo);

    public override Number Return() => OnePlusTwo;
}
