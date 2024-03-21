using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Graphviz;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Json;

// File names
string
    fileName = "fluent-calculations-demo",
    jsonFileName = $"{fileName}.json",
    dotFileName = $"{fileName}.dot",
    pngFileName = $"{dotFileName}.png";

// Run the calculation
Number resultValue = new Demo().ToResult();

// Serialize to Json
string resultAsJson = ValueJsonSerializer.Serialize(resultValue);
File.WriteAllText(jsonFileName, resultAsJson);

// Deserialize from json
IValue resultFromJson = ValueJsonSerializer.Deserialize(resultAsJson);

// Convert to graph
DotGraph dotGraph = new DotGraphValueBuilder().Build(resultFromJson);

// Save graph to .dot format
await new DotGraphToFileWriter().SaveToDot(dotGraph, dotFileName);

// Convert .dot graph to PNG image using Graphviz tool
Graphviz.ConvertToPNG(dotFileName);

Console.WriteLine($@"Result file name ""{pngFileName}""");

// Demo calculation
public class Demo : EvaluationScope<Number>
{
    Number
        A = Number.Of(1),
        B = Number.Of(2),
        C = Number.Of(3);

    Number D => Evaluate(() => A + B);

    Number E => Evaluate(() => D * C);

    public override Number Return() => E;
}
