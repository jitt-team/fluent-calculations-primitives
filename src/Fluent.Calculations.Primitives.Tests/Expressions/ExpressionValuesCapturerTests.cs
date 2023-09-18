namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;

public class ExpressionValuesCapturerTests
{
    private readonly Mock<IMemberExpressionsCapturer> expressionMembersCapturerMock;
    private readonly Mock<IReflectionProvider> reflectionProviderMock;

    public ExpressionValuesCapturerTests() => expressionMembersCapturerMock = new Mock<IMemberExpressionsCapturer>(MockBehavior.Strict);

    [Fact(Skip ="Needs updating")]
    public void CapturedMembers_AreReturned()
    {
        Number
            dummyValue = Number.Of(1);

        CapturedParameter
            testParam1 = new CapturedParameter(Number.Zero, "INPUT-MEMBER-1"),
            testParam2 = new CapturedParameter(Number.Zero, "INPUT-MEMBER-2");

        CapturedEvaluation
            testEval1 = new CapturedEvaluation("EVALUATION-MEMBER-1"),
            testEval2 = new CapturedEvaluation("EVALUATION-MEMBER-2");

        CapturedExpressionValues result = BuildCapturer().Capture(() => dummyValue);

        result.Should().NotBeNull();
        result.Parameters.Count().Should().Be(2);
        result.Parameters[0].Name.Should().Be(testParam1.Name);
        result.Parameters[1].Name.Should().Be(testParam2.Name);

        result.Evaluations.Count().Should().Be(2);
        result.Evaluations[0].Name.Should().Be(testEval1.Name);
        result.Evaluations[1].Name.Should().Be(testEval2.Name);

        ExpressionValuesCapturer BuildCapturer() => BuildExpressionCapturer(
            new[] { testParam1, testParam2 },
            new[] { testEval1, testEval2 });
    }

    ExpressionValuesCapturer BuildExpressionCapturer(CapturedParameter[] inputMembers, CapturedEvaluation[] evaulationMembers)
    {
        //List<object> returnValue = inputMembers.Cast<object>().Concat(evaulationMembers.Cast<object>()).ToList();
        //expressionMembersCapturer.Setup(e => e.Capture(It.IsAny<Expression>())).Returns(returnValue);
        return new ExpressionValuesCapturer(expressionMembersCapturerMock.Object, null);
    }
}
