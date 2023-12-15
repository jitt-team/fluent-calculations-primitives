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

        private readonly RelatedCalculation Releated = new RelatedCalculation();

        private readonly Number
            ValueOne = Number.Of(30),
            ValueTwo = Number.Of(20);

        private readonly Option<SomeOptions>
            SomeChoice = Option.Of(SomeOptions.OptionOne),
            OtherChoice = Option.Of(SomeOptions.OptionTwo);

        Condition OptionsEqual => Evaluate(() => SomeChoice == OtherChoice);

        Condition FirstIsGreaterThanTwo => Evaluate(() => ValueOne > ValueTwo);

        Number ResultOne() => Evaluate(() => FirstIsGreaterThanTwo && OptionsEqual ? ValueOne : Releated.CalculateRelated());

        Number SwitchResult => Evaluate(() => SomeChoice.Switch<Number>()
                .Case(SomeOptions.OptionOne, SomeOptions.OptionTwo)
                    .Return(ResultOne)
                .Case(SomeOptions.OptionThree)
                    .Return(10)
                    .Default(100));

        public override Number Return() => SwitchResult;
    }

    public class RelatedCalculation
    {
        public Number CalculateRelated()
        {
            var scope = this.GetScope();

            Number
                A = Number.Of(5),
                B = Number.Of(3);

            return scope.Evaluate(() => A * B);
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