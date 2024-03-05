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
namespace Fluent.Calculations.Graphviz
{
    public class DemoCalculation : EvaluationScope<Number>
    {
        public DemoCalculation() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true, Scope = "DemoCalculation" }) { }

        readonly RelatedCalculation ChildCalculation = new();

        readonly Number
            ValueOne = Number.Of(30),
            ValueTwo = Number.Of(20);

        readonly Option<SomeOptions>
             SomeChoice = Option.Of(SomeOptions.OptionOne),
             OtherChoice = Option.Of(SomeOptions.OptionTwo);

        Condition OptionsEqual => Evaluate(() => SomeChoice == OtherChoice);

        Condition FirstIsGreaterThanTwo => Evaluate(() => ValueOne > ValueTwo);

        Number ResultOne() => Evaluate(() => FirstIsGreaterThanTwo && OptionsEqual ?
            ValueOne : ChildCalculation.Calculate());

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
        public Number Calculate()
        {
            var scope = Scope.CreateHere(this);

            Number
                A = Number.Of(5),
                B = Number.Of(3);

            var result = scope.Evaluate(() => A * B);

            return result;
        }
    }

    public enum SomeOptions
    {
        NotSet = 1,
        OptionOne = 2,
        OptionTwo = 3,
        OptionThree = 4,
    }

    public class IsHealthyBodyMassIndex : EvaluationScope<Condition>
    {
        Number
            Weight = Number.Of(80),
            Height = Number.Of(1.80m),
            Square = Number.Of(2);

        Number
            HealthyRangeFrom = Number.Of(20),
            HealthyRangeTo = Number.Of(30);

        Number BMI => Evaluate(() => Weight / (Height ^ Square));

        Condition IsHealthy => Evaluate(() => HealthyRangeFrom <= BMI && BMI <= HealthyRangeTo);

        public override Condition Return() => IsHealthy;
    }
}