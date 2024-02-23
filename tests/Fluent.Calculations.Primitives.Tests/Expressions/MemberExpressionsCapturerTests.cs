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

        MemberExpression[] captureResult = capturer.Capture(() => TestClass.TestFieldOne + TestClass.TestFieldTwo + TestClass.TestEvaluationOne + TestClass.TestEvaluationTwo);

        string[] expectedMemberNames = [
            nameof(TestClass.TestFieldOne),
            nameof(TestClass.TestFieldTwo),
            nameof(TestClass.TestEvaluationOne),
            nameof(TestClass.TestEvaluationTwo)
        ];

        captureResult.Length.Should().Be(4);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void LambdaWithDublicateMembers_IValue_UniqueAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => TestClass.TestFieldOne + TestClass.TestFieldOne + TestClass.TestEvaluationOne + TestClass.TestEvaluationOne);

        string[] expectedMemberNames = [
            nameof(TestClass.TestFieldOne),
            nameof(TestClass.TestEvaluationOne)
        ];

        captureResult.Length.Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void LambdaWithUnsupportedMembers_UnsupportedAreIgnored()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();
        decimal unsupportedValueType = 1;
        MemberExpression[] captureResult = capturer.Capture(() => unsupportedValueType > 2 ? TestClass.TestFieldOne : TestClass.TestFieldTwo);

        string[] expectedMemberNames = [
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo)
        ];

        captureResult.Length.Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void Lambda_WithVariousMemberTypes_AllAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => TestClass.TestCondition ? TestClass.TestFieldOne : TestClass.TestFieldTwo);

        string[] expectedMemberNames = [
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo),
            nameof(testClass.TestCondition)
        ];

        captureResult.Length.Should().Be(3);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    [Fact]
    public void Lambda_CustomFunction_AllAreCaptured()
    {
        MemberExpressionsCapturer capturer = new();
        TestClass testClass = new();

        MemberExpression[] captureResult = capturer.Capture(() => CustomFunction(TestClass.TestFieldOne, TestClass.TestFieldTwo));

        string[] expectedMemberNames = [
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo)
        ];

        captureResult.Length.Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }

    private static Number CustomFunction(Number val1, Number val2) => val1 + val2;
}

public class TestClass
{
    public static Number TestFieldOne => Number.Of(5.00m);

    public static Number TestFieldTwo => Number.Of(6.00m);

    public static Number TestEvaluationOne => Number.Of(7.00m);

    public static Number TestEvaluationTwo => Number.Of(8.00m);

    public static Condition TestCondition => Condition.True();
}
