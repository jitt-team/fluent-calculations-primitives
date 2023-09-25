﻿namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class BinaryExpressionOperatorTranslatorTests
{
    [Fact]
    public void TestExistingMethod_IsTranslatedCorrectly() => BinaryExpressionOperatorTranslator
        .MethodNameToOperator("Add").Should().Be("+");

    [Fact]
    public void TestNotExistingMethod_ReturnsUnknown() => BinaryExpressionOperatorTranslator
        .MethodNameToOperator("NonExistentMethodName").Should().Be(BinaryExpressionOperatorTranslator.Unknown);

}
