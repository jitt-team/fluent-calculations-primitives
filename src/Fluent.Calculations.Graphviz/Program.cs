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

// Run the calculation
Condition resultValue = new IsHealthyBodyMassIndex().ToResult();

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
public class IsHealthyBodyMassIndex : EvaluationScope<Condition>
{
    Number
        WeightKg = Number.Of(80),
        HeightMeters = Number.Of(1.80m),
        Square = Number.Of(2);

    Number
        HealthyBMIFrom = Number.Of(20),
        HealthyBMITo = Number.Of(30);

    Number BMI => Evaluate(() => WeightKg / (HeightMeters ^ Square));

    Condition IsWithinHealthyBMIRange => Evaluate(() => HealthyBMIFrom <= BMI && BMI <= HealthyBMITo);

    public override Condition Return() => IsWithinHealthyBMIRange;
}
