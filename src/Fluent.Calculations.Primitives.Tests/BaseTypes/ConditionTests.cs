using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
public class ConditionTests
{
    [Fact(DisplayName = "Condition instance of True should equal Boolean True")]
    public void Should_be_true() => Condition.True().IsTrue.Should().BeTrue();

    [Fact]
    public void Should_be_false() => Condition.False().IsTrue.Should().BeFalse();

    [Fact]
    public void Greater_with_left_greater_should_be_true()
    {
        Condition result = Number.Of(10) > Number.Of(1);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void Lesser_with_left_greater_should_be_false()
    {
        Condition result = Number.Of(10) < Number.Of(1);
        result.IsTrue.Should().BeFalse();
    }

    [Fact]
    public void Equalling_non_equal_should_be_false()
    {
        Condition result = Number.Of(10) == Number.Of(1);
        result.IsTrue.Should().BeFalse();
    }

    [Fact]
    public void Equalling_equal_should_be_true()
    {
        Condition result = Number.Of(10) == Number.Of(10);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void NonEqualling_not_equal_should_be_true()
    {
        Condition result = Number.Of(10) != Number.Of(1);
        result.IsTrue.Should().BeTrue();
    }

    [Fact]
    public void NonEqualling_equal_should_be_false()
    {
        Condition result = Number.Of(10) != Number.Of(10);
        result.IsTrue.Should().BeFalse();
    }
}
