using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Json;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.DotNetGraph
{
    public class CalculationDotGraphRendererTests
    {
        [Fact]
        public void AdditionCalculation_Rendered_GraphHasExpectedElements()
        {
            DotGraph graph = new DotGraphValueBuilder().Build(RunCalculation());
            graph.Elements.Should().HaveCount(5);
        }

        [Fact]
        public void RenderingThrough_JsonSerialization_SameGraph()
        {
            Number result = RunCalculation();
            DotGraph graphFromResult = new DotGraphValueBuilder().Build(result);

            IValue valueFromJson = ValueJsonConverter.Deserialize(ValueJsonConverter.Serialize(result));            
            DotGraph graphFromJson = new DotGraphValueBuilder().Build(valueFromJson);

            graphFromJson.Elements.Should().HaveCount(graphFromResult.Elements.Count);

            for (int elementIndex = 0; elementIndex < graphFromJson.Elements.Count; elementIndex++)
            {
                DotElement actual = (DotElement)graphFromJson.Elements[elementIndex];
                DotElement expected = (DotElement)graphFromResult.Elements[elementIndex];
                actual.Label?.Value.Should().Be(expected.Label.Value);
            }
        }

        private Number RunCalculation() => new AdditionCalculation
        {
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3)
        }.ToResult();
    }

    internal class AdditionCalculation : EvaluationScope<Number>
    {
        public AdditionCalculation() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero;

        Number OnePlusTwo => Evaluate(() => ConstantOne + ConstantTwo);

        public override Number Return() => OnePlusTwo;
    }
}
