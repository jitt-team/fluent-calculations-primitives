namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class BinaryExpressionOperatorTranslatorTests
{
    [Fact]
    public void TestExistingMethod_IsTranslatedCorrectly() => LanguageOperatorTranslator
        .MethodNameToOperator("Add").Should().Be("+");

    [Fact]
    public void TestNotExistingMethod_ReturnsUnknown() => LanguageOperatorTranslator
        .MethodNameToOperator("NonExistentMethodName").Should().Be(LanguageOperatorTranslator.Unknown);
}
