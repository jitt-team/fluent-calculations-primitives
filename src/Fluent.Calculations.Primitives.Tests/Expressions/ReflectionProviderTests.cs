namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using System.Linq.Expressions;

public class ReflectionProviderTests
{
    readonly ReflectionProvider memberAccessReflectionProvider = new();

    [Fact]
    public void HasValue_ReturnsResult()
    {
        Number testValue = Number.Of(1, "TEST-VALUE");
        Expression<Func<Number>> testExpression = () => testValue;
        IValueProvider result = memberAccessReflectionProvider.GetValue(testExpression.Body);
        result.Name.Should().Be(testValue.Name);
    }

    [Fact]
    public void NullValue_Throws()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Number testValue = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        Expression<Func<Number>> testExpression = () => testValue;
#pragma warning restore CS8603 // Possible null reference return.
        Assert.Throws<NullExpressionResultException>(() => memberAccessReflectionProvider.GetValue(testExpression.Body));

    }
}
