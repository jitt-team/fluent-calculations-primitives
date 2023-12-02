using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.BaseTypes
{
    public class OptionTests
    {
        [Fact]
        public void Compare_EqualOptions_True()
        {
            Option<TestEnum>
                left = Option.Of(TestEnum.First),
                right = Option.Of(TestEnum.First);

            Condition equal = left == right;

            equal.IsTrue.Should().BeTrue();
        }

        [Fact]
        public void Compare_NonEqualOptions_False()
        {
            Option<TestEnum>
                left = Option.Of(TestEnum.First),
                right = Option.Of(TestEnum.Second);

            Condition equal = left == right;

            equal.IsTrue.Should().BeFalse();
        }
    }

    public enum TestEnum
    {
        First,
        Second,
        Third
    }
}
