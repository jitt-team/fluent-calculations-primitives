using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
using System.Xml.Linq;

namespace Fluent.Calculations.Primitives.Tests
{
    public class Experiments
    {

        [Fact]
        public void Test()
        {
            Number Value = 20;

            // Number result = Result.Of();
            //Number result = SwitchExpression<Number, Number>
            //    .Switch(Value)
            //        .Case(Number.Of(10)).Return(Number.Of(15))
            //        .Case(Number.Of(20)).Return(Number.Of(30))
            //        .Default(Number.Of(20));

            //EvaluationContext<Number> ctx = new();

            //Number someResult = ctx.Switch(Value)
            //            .Case(Number.Of(10)).Return(Number.Of(15))
            //            .Case(Number.Of(20)).Return(Number.Of(30))
            //            .Default(Number.Of(20));

            Number someResult = SwitchExpression<Number>.For(Value)
                    .Case(Number.Of(10)).Return(Number.Of(15))
                    .Case(Number.Of(20)).Return(Number.Of(30))
                    .Default(Number.Of(20));

            someResult.Primitive.Should().Be(30);
        }
    }


}
