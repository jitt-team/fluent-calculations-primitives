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
Number resultValue = DemoCalculation.ScopeOne();

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

    public class DemoCalculation
    {
        public static Number ScopeOne()
        {
            Scope scope = new();
            var context = scope.CreateContext<Number>();

            Number
                Scope1Value1 = Number.Of(30, scope.Name, nameof(Scope1Value1)),
                Scope1Value2 = Number.Of(20, scope.Name, nameof(Scope1Value2));

            Number Scope1Result1 = context.Evaluate(() => Scope1Value1 + ScopeTwo(Scope1Value2), nameof(Scope1Result1));

            return Scope1Result1;
        }

        private static Number ScopeTwo(Number valueFromOtherScope)
        {
            Scope scope = new();
            var context = scope.CreateContext<Number>();

            Number
                Scope2Value1 = Number.Of(30, scope.Name, nameof(Scope2Value1));

            Number Scope2Result1 = context.Evaluate(() => valueFromOtherScope * Scope2Value1, nameof(Scope2Result1));

            return Scope2Result1;
        }
    }

    public enum SomeOptions
    {
        NotSet = 1,
        OptionOne = 2,
        OptionTwo = 3,
        OptionThree = 4,
    }
}