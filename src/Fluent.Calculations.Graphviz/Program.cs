using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Graphviz;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Json;

// File names
string
    fileName = "fluent-calculations-demo",
    jsonFileName = $"{fileName}.json",
    dotFileName = $"{fileName}.dot",
    pngFileName = $"{dotFileName}.png";

// Run the calculation
Number resultValue = new DemoCalculation().ToResult();

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

Console.WriteLine($@"Result file name: ""{pngFileName}""");