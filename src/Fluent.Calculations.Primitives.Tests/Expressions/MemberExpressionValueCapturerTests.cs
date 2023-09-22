namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using System.Reflection;

public class MemberExpressionValueCapturerTests
{
    private readonly Mock<IMemberExpressionsCapturer> expressionMembersCapturerMock;
    private readonly Mock<IReflectionProvider> reflectionProviderMock;

    public MemberExpressionValueCapturerTests()
    {
        expressionMembersCapturerMock = new Mock<IMemberExpressionsCapturer>(MockBehavior.Strict);
        reflectionProviderMock = new Mock<IReflectionProvider>(MockBehavior.Strict);
    }

    [Fact]
    public void CapturedMembers_AreReturned()
    {
        var testClass = new TestClass();

        // Expression<Func<Func<Number>>> testEvaluationExpression = () => testClass.TestEvaluation;

        MemberExpressionValues result = BuildCapturer().Capture(() => testClass.TestParameter);

        result.Parameters.Should().HaveCount(1);

        MemberExpressionValueCapturer BuildCapturer() => BuildExpressionCapturer(testClass);
    }

    public class TestClass
    {
        public Number TestParameter = Number.Of(1, "TEST-PARAMETE");
        public Func<Number> TestEvaluation = () => Number.Of(2, "TEST-EVALUATION");
    }

    MemberExpressionValueCapturer BuildExpressionCapturer(TestClass testClass)
    {
        expressionMembersCapturerMock.Setup(c => c.Capture(It.IsAny<Expression>())).Returns(new List<MemberExpression> { GetLambdaBody(() => testClass.TestParameter) });
        reflectionProviderMock.Setup(p => p.IsParameter(It.IsAny<MemberInfo>())).Returns(true);
        reflectionProviderMock.Setup(p => p.GetValue(It.IsAny<Expression>())).Returns(testClass.TestParameter);
        reflectionProviderMock.Setup(p => p.GetPropertyOrFieldName(It.IsAny<MemberInfo>())).Returns(nameof(testClass.TestParameter));

        return new MemberExpressionValueCapturer(expressionMembersCapturerMock.Object, reflectionProviderMock.Object);
    }

    MemberExpression GetLambdaBody(Expression expression) => (MemberExpression)((LambdaExpression)expression).Body;
}
