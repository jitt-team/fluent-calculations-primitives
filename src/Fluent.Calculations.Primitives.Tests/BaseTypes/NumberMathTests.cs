namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

public class NumberMathTests
{
    [Fact]
    public void Number_MathAbs_IsExpectedResult()
    {
        Number value = Number.Of(-3.50m, "INPUT-VALUE");
        Number result = NumberMath.Abs(value);
        result.Primitive.Should().Be(3.50m);
    }
}
