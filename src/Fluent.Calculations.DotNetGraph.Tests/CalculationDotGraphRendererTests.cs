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
            string json = ValueJsonConverter.Serialize(result);
            IValue valueFromJson = ValueJsonConverter.Deserialize(json);
            DotGraph graph = new DotGraphValueBuilder().Build(valueFromJson);
            graph.Elements.Should().HaveCount(5);
        }

        private Number RunCalculation() => new AdditionCalculation
        {
            ConstantOne = Number.Of(2),
            ConstantTwo = Number.Of(3)
        }.ToResult();
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
}
