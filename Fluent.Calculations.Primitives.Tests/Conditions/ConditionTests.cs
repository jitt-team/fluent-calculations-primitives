using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Conditions;
public class ConditionTests
{
    [Fact]
    public void should_be_true() => Condition.True().IsTrue.Should().BeTrue();

    [Fact]
    public void should_be_false() => Condition.False().IsTrue.Should().BeFalse();

    [Fact]
    public void greater_than_should_work()
    {
        var result = Number.Of(10) > Number.Of(1);

        result.Should().BeOfType<Condition>();

        ((bool)result).Should().BeTrue();
    }

    [Fact]
    public void less_than_should_work()
    {
        var result = Number.Of(10) < Number.Of(1);

        result.Should().BeOfType<Condition>();

        ((bool)result).Should().BeFalse();
    }

    [Fact]
    public void equal_should_work()
    {
        var result = Number.Of(10) == Number.Of(1);

        result.Should().BeOfType<Condition>();

        ((bool)result).Should().BeFalse();
    }


    [Fact]
    public void not_equal_should_work()
    {
        var result = Number.Of(10) != Number.Of(1);

        result.Should().BeOfType<Condition>();

        ((bool)result).Should().BeTrue();
    }
}
