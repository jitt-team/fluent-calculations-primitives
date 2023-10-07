namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using System.Linq.Expressions;

public class MemberExpressionsCapturerTests
{
    [Fact]
    public void LambdaWithMultipleMembers_IValue_AllAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => testClass.TestFieldOne + testClass.TestFieldTwo + testClass.TestEvaluationOne + testClass.TestEvaluationTwo);

        string[] expectedMemberNames = new[] {
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo),
            nameof(testClass.TestEvaluationOne),
            nameof(testClass.TestEvaluationTwo)
        };

        captureResult.Count().Should().Be(4);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void LambdaWithDublicateMembers_IValue_UniqueAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => testClass.TestFieldOne + testClass.TestFieldOne + testClass.TestEvaluationOne + testClass.TestEvaluationOne);

        string[] expectedMemberNames = new[] {
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestEvaluationOne)
        };

        captureResult.Count().Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void LambdaWithUnsupportedMembers_UnsupportedAreIgnored()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();
        decimal unsupportedValueType = 1;
        MemberExpression[] captureResult = capturer.Capture(() => unsupportedValueType > 2 ? testClass.TestFieldOne : testClass.TestFieldTwo);

        string[] expectedMemberNames = new[] {
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo)
        };

        captureResult.Count().Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void Lambda_WithVariousMemberTypes_AllAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => testClass.TestCondition ? testClass.TestFieldOne : testClass.TestFieldTwo);

        string[] expectedMemberNames = new[] {
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo),
            nameof(testClass.TestCondition)
        };

        captureResult.Count().Should().Be(3);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }
}

public class TestClass
{
    public Number TestFieldOne = Number.Of(5.00m);

    public Number TestFieldTwo = Number.Of(6.00m);

    public Number TestEvaluationOne => Number.Of(7.00m);

    public Number TestEvaluationTwo => Number.Of(8.00m);

    public Condition TestCondition => Condition.True();
}
