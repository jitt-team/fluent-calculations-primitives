using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;

// Demo calculation
namespace Fluent.Calculations.Graphviz
{
    public enum SomeOptions
    {
        NotSet = 1,
        OptionOne = 2,
        OptionTwo = 3,
        OptionThree = 4,
    }

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
}