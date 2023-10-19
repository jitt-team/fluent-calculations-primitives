using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
public class ConditionTests
{
    [Fact(DisplayName = "Condition instance of True should equal Boolean True")]
    public void should_be_true() => Condition.True().IsTrue.Should().BeTrue();

    [Fact]
    public void should_be_false() => Condition.False().IsTrue.Should().BeFalse();

    [Fact]
    public void greater_with_left_greater_should_be_true()
    {
        Condition result = Number.Of(10) > Number.Of(1);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void lesser_with_left_greater_should_be_false()
    {
        Condition result = Number.Of(10) < Number.Of(1);
        result.IsTrue.Should().BeFalse();
    }

    [Fact]
    public void equalling_non_equal_should_be_false()
    {
        Condition result = Number.Of(10) == Number.Of(1);
        result.IsTrue.Should().BeFalse();
    }

    [Fact]
    public void equalling_equal_should_be_true()
    {
        Condition result = Number.Of(10) == Number.Of(10);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void nonEqualling_not_equal_should_be_true()
    {
        Condition result = Number.Of(10) != Number.Of(1);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void nonEqualling_equal_should_be_false()
    {
        Condition result = Number.Of(10) != Number.Of(10);
        result.IsTrue.Should().BeFalse();
    }
}
