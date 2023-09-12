namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Xunit.Abstractions;

public class ExpressionCapturerTests
{
    private readonly Mock<IExpressionMembersCapturer> expressionMembersCapturer;

    public ExpressionCapturerTests()
    {
        expressionMembersCapturer = new Mock<IExpressionMembersCapturer>(MockBehavior.Strict);
    }

    [Fact]
    public void test()
    {
        Condition a = Condition.True();
        Number
            b = Number.Of(1),
            c = Number.Of(2);

        ExpressionCaptureResult result = BuildExpressionCapturer().Capture(() => a ? b : c);
        result.Should().NotBeNull();
    }

    ExpressionCapturer BuildExpressionCapturer()
    {
        expressionMembersCapturer.Setup(e => e.Capture(It.IsAny<Expression>())).Returns(new List<object>());

        return new ExpressionCapturer(expressionMembersCapturer.Object);
    }
}
