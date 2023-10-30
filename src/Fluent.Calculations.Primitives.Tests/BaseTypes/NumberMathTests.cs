namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Tests.ComplexValueType;
using FluentAssertions;

public class NumberMathTests
{
    [Fact]
    public void Number_MathAbs_IsExpectedResult()
    {
        string expectedArgumentName = "INPUT-VALUE";
        Number value = Number.Of(-3, expectedArgumentName);

        Number result = ValueMath.Abs(value);

        result.Primitive.Should().Be(3);
        result.Name.Should().Be("Abs");
        result.Expression.Body.Should().Be($"Abs({expectedArgumentName})");
        result.Expression.Arguments.Should().HaveCount(1);
        result.Expression.Arguments.Single().Name.Should().Be(expectedArgumentName);
        result.Expression.Arguments.Single().Primitive.Should().Be(-3);
    }

    [Fact]
    public void Money_MathAbs_IsExpectedResult()
    {
        string expectedArgumentName = "INPUT-VALUE";
        Money value = Number.Of(-3, expectedArgumentName).AsMoney().EUR;

        Money result = ValueMath.Abs(value);

        result.Primitive.Should().Be(3);
        result.Name.Should().Be("Abs");
        result.Expression.Body.Should().Be($"Abs({expectedArgumentName} (EUR))");
        result.Expression.Arguments.Should().HaveCount(1);
        result.Expression.Arguments.Single().Name.Should().Be(expectedArgumentName);
        result.Expression.Arguments.Single().Primitive.Should().Be(-3);
    }

    [Fact]
    public void Number_MathMax_IsExpectedResult()
    {
        string
            expectedArgumentName1 = "INPUT-VALUE-1",
            expectedArgumentName2 = "INPUT-VALUE-2";

        Number
            value1 = Number.Of(2, expectedArgumentName1),
            value2 = Number.Of(3, expectedArgumentName2);

        Number result = ValueMath.Max(value1, value2);

        result.Primitive.Should().Be(3);
        result.Name.Should().Be("Max");
        result.Expression.Body.Should().Be($"Max({expectedArgumentName1},{expectedArgumentName2})");
        result.Expression.Arguments.Should().HaveCount(2);
        result.Expression.Arguments.First().Name.Should().Be(expectedArgumentName1);
        result.Expression.Arguments.First().Primitive.Should().Be(2);
        result.Expression.Arguments.Last().Name.Should().Be(expectedArgumentName2);
        result.Expression.Arguments.Last().Primitive.Should().Be(3);
    }

    [Fact]
    public void Number_MathROund_IsExpectedResult()
    {
        string
            expectedArgumentName1 = "INPUT-VALUE-1",
            expectedArgumentName2 = "INPUT-VALUE-2";

        Number
            value1 = Number.Of(2.247m, expectedArgumentName1),
            value2 = Number.Of(2, expectedArgumentName2);

        Number result = ValueMath.Round(value1, value2);

        result.Primitive.Should().Be(2.25m);
        result.Name.Should().Be("Round");
        result.Expression.Body.Should().Be($"Round({expectedArgumentName1},{expectedArgumentName2})");
        result.Expression.Arguments.Should().HaveCount(2);
        result.Expression.Arguments.First().Name.Should().Be(expectedArgumentName1);
        result.Expression.Arguments.First().Primitive.Should().Be(2.247m);
        result.Expression.Arguments.Last().Name.Should().Be(expectedArgumentName2);
        result.Expression.Arguments.Last().Primitive.Should().Be(2);
    }
}
