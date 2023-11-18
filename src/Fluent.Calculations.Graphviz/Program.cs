using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Graphviz;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

// Run the calculation (See implementation below)
IValue resultValue = new DemoCalculation().ToResult();

// Convert to graph
DotGraph dotGraph = new CalculationDotGraphRenderer().Render(resultValue);

// Save graph to .dot format
string dotFilePath = "fluent-calculation-demo.dot";
await new DotGraphWriterFile().SaveToDot(dotGraph, dotFilePath);

// Convert .dot graph to PNG image using Graphviz tool
new Graphviz().ConvertToPNG(dotFilePath);

Console.WriteLine($@"Result file name ""{dotFilePath}.png""");

// Demo calculation
namespace Fluent.Calculations.Graphviz
{
    public class DemoCalculation : EvaluationContext<Number>
    {
        public DemoCalculation() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        private Number
            ValueOne = Number.Of(30),
            ValueTwo = Number.Of(20),
            ValueThree = Number.Of(2);

        public Condition FirstIsGreaterThanTwo => Evaluate(() => ValueOne > ValueTwo);

        Number ResultOne => Evaluate(() => FirstIsGreaterThanTwo ? ValueOne - ValueTwo : ValueOne + ValueTwo);

        Number FinalResult => Evaluate(() => ResultOne * ValueThree);

        public override Number Return() => FinalResult;
    }
}