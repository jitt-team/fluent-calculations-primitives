using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Conditions;
public class ConditionTests
{
    [Fact]
    public void should_be_true_0()
    {
        bool result = Condition.True();

        //works only with explicit casting
        result.Should().Be(true);

        //this one is goint to fail
        //Condition.True().Should().Be(true);
    }

    [Fact]
    public void should_be_true() =>
       //As uses is operator under the hood which we can not override
       //Condition.False().As<bool>().Should().BeFalse();
       ((bool)Condition.False()).Should().BeFalse();

    [Fact]
    public void should_be_false() =>
        ((bool)Condition.True()).Should().BeTrue();

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
