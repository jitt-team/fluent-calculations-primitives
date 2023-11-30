using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Switch
{
    public class SwitchTests
    {
        [Fact]
        public void SimpleSwitch_ReturnsCorrectItem()
        { 
            Number 
                NumberOne = Number.Of(5, nameof(NumberOne)),
                NumberTwo = Number.Of(10, nameof(NumberTwo)),
                NumberThree = Number.Of(15, nameof(NumberThree));

            Number NumberSum = NumberTwo + NumberThree;

            Number SwitchResult = SwitchExpression<Number>.For(NumberOne)
                .Case(Number.Of(1)).Return(Number.Of(10))
                .Case(Number.Of(3)).Return(Number.Of(30))
                .Case(Number.Of(5)).Return(NumberSum)
                .Default(Number.Of(100), nameof(SwitchResult));

            SwitchResult.Name.Should().Be(nameof(SwitchResult));
            SwitchResult.Primitive.Should().Be(50);
        }
    }
}
