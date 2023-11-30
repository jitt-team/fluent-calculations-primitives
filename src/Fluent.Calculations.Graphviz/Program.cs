using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Graphviz;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Json;

// File names
string
    fileName = "fluent-calculation-demo",
    jsonFileName = $"{fileName}.json",
    dotFileName = $"{fileName}.dot",
    pngFileName = $"{dotFileName}.png";

// Run the calculation (See implementation below)
Number resultValue = new DemoCalculation().ToResult();

// Serialize to Json
string resultAsJson = ValueJsonConverter.Serialize(resultValue);
File.WriteAllText(jsonFileName, resultAsJson);

// Deserialize from json
IValue resultFromJson = ValueJsonConverter.Deserialize(resultAsJson);

// Convert to graph
DotGraph dotGraph = new DotGraphValueBuilder().Build(resultFromJson);

// Save graph to .dot format
await new DotGraphToFileWriter().SaveToDot(dotGraph, dotFileName);

// Convert .dot graph to PNG image using Graphviz tool
Graphviz.ConvertToPNG(dotFileName);

Console.WriteLine($@"Result file name ""{pngFileName}""");

// Demo calculation
namespace Fluent.Calculations.Graphviz
{
    public class DemoCalculation : EvaluationContext<Number>
    {
        public DemoCalculation() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        private readonly Number
            ValueOne = Number.Of(30),
            ValueTwo = Number.Of(20),
            ValueThree = Number.Of(2);

        public Condition FirstIsGreaterThanTwo => Evaluate(() => ValueOne > ValueTwo);

        Number ResultOne => Evaluate(() => FirstIsGreaterThanTwo ? ValueOne : ValueTwo);

        Number OtherResult => Evaluate(() => ResultOne * ValueThree);

        Number SwitchResult => Evaluate(() =>
            Switch(OtherResult)
                .Case(Number.Of(20)).Return(Number.Of(10))
                .Case(Number.Of(40)).Return(Number.Of(30))
                .Case(Number.Of(60)).Return(ResultOne)
                .Default(Number.Of(100)));

        public override Number Return() => SwitchResult;
    }
}