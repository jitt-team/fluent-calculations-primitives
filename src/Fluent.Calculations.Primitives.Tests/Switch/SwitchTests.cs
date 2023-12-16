using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Switch
{
    public class SwitchTests
    {
        [Fact]
        public void SimpleSwitch_ExistinCase_ReturnsCorrectItem()
        {
            Option<TestEnum> TestOption = Option.Of(TestEnum.Second, nameof(TestOption));

            Number SwitchResult = SwitchExpression<TestEnum, Number>.For(TestOption)
                .Case(TestEnum.First).Return(10)
                .Case(TestEnum.Second).Return(20)
                .Default(40)
                .GetResult(nameof(SwitchResult));

            SwitchResult.Name.Should().Be(nameof(SwitchResult));
            SwitchResult.Primitive.Should().Be(20);
            SwitchResult.Expression.Arguments.Count.Should().Be(1);
            SwitchResult.Expression.Arguments.Single().Name.Should().Be(nameof(TestOption));
            TestEnum resultArgumentPrimitive = (Option<TestEnum>)SwitchResult.Expression.Arguments.Single();
            resultArgumentPrimitive.Should().Be(TestEnum.Second);
        }

        [Fact]
        public void SimpleSwitch_Default_ReturnsCorrectItem()
        {
            Option<TestEnum> TestOption = Option.Of(TestEnum.Third);

            Number SwitchResult = SwitchExpression<TestEnum, Number>.For(TestOption)
                .Case(TestEnum.First).Return(10)
                .Case(TestEnum.Second).Return(20)
                .Default(40)
                .GetResult(nameof(SwitchResult));

            SwitchResult.Name.Should().Be(nameof(SwitchResult));
            SwitchResult.Primitive.Should().Be(40);
        }

        [Fact]
        public void SimpleSwitch_WithCalculatedReturn_IncludedAsArgumet()
        {
            Option<TestEnum>
                TestOption = Option.Of(TestEnum.Second, nameof(TestOption));

            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            Number SomeResultNumber() => Result.Of(() => NumberOne + NumberTwo, nameof(SomeResultNumber));

            Number SwitchResult = SwitchExpression<TestEnum, Number>.For(TestOption)
                .Case(TestEnum.First).Return(10)
                .Case(TestEnum.Second).Return(SomeResultNumber)
                .Default(40)
                .GetResult(nameof(SwitchResult));

            SwitchResult.Expression.Arguments.Count.Should().Be(2);
            SwitchResult.Expression.Arguments.Last().Name.Should().Be(nameof(SomeResultNumber));
        }
    }

    public enum TestEnum
    {
        First,
        Second,
        Third
    }
}
