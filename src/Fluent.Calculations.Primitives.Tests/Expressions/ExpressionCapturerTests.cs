namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

public class ExpressionCapturerTests
{
    private readonly Mock<IExpressionMembersCapturer> expressionMembersCapturer;

    public ExpressionCapturerTests() => expressionMembersCapturer = new Mock<IExpressionMembersCapturer>(MockBehavior.Strict);

    [Fact]
    public void CapturedMembers_AreReturned()
    {
        Number
            dummyValue = Number.Of(1);

        CapturedInputMember
            testInputMember1 = new CapturedInputMember(Number.Zero, "INPUT-MEMBER-1"),
            testInputMember2 = new CapturedInputMember(Number.Zero, "INPUT-MEMBER-2");

        CapturedEvaulationMember
            testEvalMember1 = new CapturedEvaulationMember("EVALUATION-MEMBER-1"),
            testEvalMember2 = new CapturedEvaulationMember("EVALUATION-MEMBER-2");

        ExpressionCaptureResult result = BuildCapturer().Capture(() => dummyValue);

        result.Should().NotBeNull();
        result.InputMembers.Count().Should().Be(2);
        result.InputMembers[0].Name.Should().Be(testInputMember1.Name);
        result.InputMembers[1].Name.Should().Be(testInputMember2.Name);

        result.EvaluationMembers.Count().Should().Be(2);
        result.EvaluationMembers[0].Name.Should().Be(testEvalMember1.Name);
        result.EvaluationMembers[1].Name.Should().Be(testEvalMember2.Name);

        ExpressionCapturer BuildCapturer() => BuildExpressionCapturer(
            new[] { testInputMember1, testInputMember2 },
            new[] { testEvalMember1, testEvalMember2 });
    }

    ExpressionCapturer BuildExpressionCapturer(CapturedInputMember[] inputMembers, CapturedEvaulationMember[] evaulationMembers)
    {
        List<object> returnValue = inputMembers.Cast<object>().Concat(evaulationMembers.Cast<object>()).ToList();
        expressionMembersCapturer.Setup(e => e.Capture(It.IsAny<Expression>())).Returns(returnValue);
        return new ExpressionCapturer(expressionMembersCapturer.Object);
    }
}
