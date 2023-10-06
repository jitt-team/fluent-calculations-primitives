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
        var capturer = new MemberExpressionsCapturer();
        var testClass = new TestClass();

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
        var capturer = new MemberExpressionsCapturer();
        var testClass = new TestClass();

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
        var capturer = new MemberExpressionsCapturer();
        var testClass = new TestClass();
        decimal unsupportedValueType = 1;
        MemberExpression[] captureResult = capturer.Capture(() => unsupportedValueType > 2 ? testClass.TestFieldOne : testClass.TestFieldTwo);

        string[] expectedMemberNames = new[] {
            nameof(testClass.TestFieldOne),
            nameof(testClass.TestFieldTwo)
        };

        captureResult.Count().Should().Be(2);
        captureResult.Should().Contain(e => expectedMemberNames.Contains(e.Member.Name));
    }
}

public class TestClass
{
    public Number TestFieldOne = Number.Of(5.00m);

    public Number TestFieldTwo = Number.Of(6.00m);

    public Number TestEvaluationOne => Number.Of(7.00m);

    public Number TestEvaluationTwo => Number.Of(8.00m);
}
