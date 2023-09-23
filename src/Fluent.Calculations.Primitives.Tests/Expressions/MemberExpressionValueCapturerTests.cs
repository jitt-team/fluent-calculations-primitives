namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System;
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
    public void CaptureParameterMember_IsReturned()
    {
        var testMemberClass = new TestMembersClass();
        MemberExpressionValues result = BuildCapturer().Capture(() => testMemberClass.TestParameter);

        result.Parameters.Should().HaveCount(1);
        result.Evaluations.Single().Name.Should().Be(TestMembersClass.TestEvaluationName);

        MemberExpressionValueCapturer BuildCapturer() => BuildTestParameterCapturer(testMemberClass);
    }

    [Fact]
    public void CaptureEvaluationMember_IsReturned()
    {
        var testMemberClass = new TestMembersClass();
        MemberExpressionValues result = BuildCapturer().Capture(() => testMemberClass.TestEvaluation);

        result.Evaluations.Should().HaveCount(1);
        result.Evaluations.Single().Name.Should().Be(TestMembersClass.TestEvaluationName);

        MemberExpressionValueCapturer BuildCapturer() => BuildTestEvaluationCapturer(testMemberClass);
    }

    public class TestMembersClass
    {
        public const string
            TestParameterName = "TEST-PARAMETER",
            TestEvaluationName = "TEST-EVALUATION";

        public Number TestParameter = Number.Of(1.00m, TestParameterName);

        public Number TestEvaluation => Number.Of(2.00m, TestEvaluationName);
    }

    MemberExpressionValueCapturer BuildTestParameterCapturer(TestMembersClass testClass)
    {
        expressionMembersCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>()))
            .Returns(new MemberExpression[] { GetLambdaBody(() => testClass.TestParameter) });

        reflectionProviderMock.Setup(p => p.IsParameter(It.IsAny<MemberInfo>())).Returns(true);
        reflectionProviderMock.Setup(p => p.GetValue(It.IsAny<Expression>())).Returns(testClass.TestParameter);
        reflectionProviderMock.Setup(p => p.GetPropertyOrFieldName(It.IsAny<MemberInfo>())).Returns(TestMembersClass.TestParameterName);

        return new MemberExpressionValueCapturer(expressionMembersCapturerMock.Object, reflectionProviderMock.Object, new ValuesCache());
    }

    MemberExpressionValueCapturer BuildTestEvaluationCapturer(TestMembersClass testClass)
    {
        expressionMembersCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>()))
            .Returns(new MemberExpression[] { GetLambdaBody(() => testClass.TestParameter) });

        reflectionProviderMock.Setup(p => p.IsParameter(It.IsAny<MemberInfo>())).Returns(false);
        reflectionProviderMock.Setup(p => p.IsEvaluation(It.IsAny<MemberInfo>())).Returns(true);
        reflectionProviderMock.Setup(p => p.GetValue(It.IsAny<Expression>())).Returns(testClass.TestEvaluation);
        reflectionProviderMock.Setup(p => p.GetPropertyOrFieldName(It.IsAny<MemberInfo>())).Returns(TestMembersClass.TestEvaluationName);

        return new MemberExpressionValueCapturer(expressionMembersCapturerMock.Object, reflectionProviderMock.Object, new ValuesCache());
    }

    MemberExpression GetLambdaBody(Expression expression) => (MemberExpression)((LambdaExpression)expression).Body;
}
