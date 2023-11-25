﻿namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using System.Linq.Expressions;

public class ReflectionProviderTests
{
    ReflectionProvider memberAccessReflectionProvider = new ReflectionProvider();

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
        Number testValue = null;
        Expression<Func<Number>> testExpression = () => testValue;
        Assert.Throws<NullExpressionResultException>(() => memberAccessReflectionProvider.GetValue(testExpression.Body));
    }
}
