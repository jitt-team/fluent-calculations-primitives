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
    public void CaptureParameterMember_NotCached_IsReturned()
    {
        var testMemberClass = new TestMembersClass();

        Mock<IValuesCache> valueCacheMock = new Mock<IValuesCache>(MockBehavior.Strict);
        valueCacheMock.Setup(c => c.ContainsKey(It.IsAny<string>())).Returns(false);
        valueCacheMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<IValue>())).Verifiable();

        MemberExpressionMembers result = BuildCapturer().CaptureMembers(() => testMemberClass.TestParameter);

        result.Parameters.Should().HaveCount(1);
        result.Parameters.Single().MemberName.Should().Be(TestMembersClass.TestParameterName);
        valueCacheMock.Verify(c => c.Add(TestMembersClass.TestParameterName, testMemberClass.TestParameter), Times.Once);

        MemberExpressionValueCapturer BuildCapturer() => BuildTestParameterCapturer(testMemberClass, valueCacheMock.Object);
    }

    [Fact]
    public void CaptureParameterMember_Cached_IsReturnedFromCache()
    {
        var testMemberClass = new TestMembersClass();
        string cachedParameterName = "CACHED-TEST-PARAMETER";

        Mock<IValuesCache> valueCacheMock = new Mock<IValuesCache>(MockBehavior.Strict);
        valueCacheMock.Setup(c => c.ContainsKey(It.IsAny<string>())).Returns(true);
        valueCacheMock.Setup(c => c.GetByKey(It.IsAny<string>())).Returns(Number.Of(3.00m, cachedParameterName));

        MemberExpressionMembers result = BuildCapturer().CaptureMembers(() => testMemberClass.TestParameter);

        result.Parameters.Should().HaveCount(1);
        result.Parameters.Single().Value.Name.Should().Be(cachedParameterName);

        MemberExpressionValueCapturer BuildCapturer() => BuildTestParameterCapturer(testMemberClass, valueCacheMock.Object);
    }

    [Fact]
    public void CaptureEvaluationMember_IsReturned()
    {
        var testMemberClass = new TestMembersClass();
        MemberExpressionMembers result = BuildCapturer().CaptureMembers(() => testMemberClass.TestEvaluation);

        result.Evaluations.Should().HaveCount(1);
        result.Evaluations.Single().MemberName.Should().Be(TestMembersClass.TestEvaluationName);

        MemberExpressionValueCapturer BuildCapturer() => BuildTestEvaluationCapturer(testMemberClass, new ValuesCache());
    }

    public class TestMembersClass
    {
        public const string
            TestParameterName = "TEST-PARAMETER",
            TestEvaluationName = "TEST-EVALUATION";

        public Number TestParameter = Number.Of(1.00m, TestParameterName);

        public Number TestEvaluation => Number.Of(2.00m, TestEvaluationName);
    }

    MemberExpressionValueCapturer BuildTestParameterCapturer(TestMembersClass testClass, IValuesCache valueCache)
    {
        expressionMembersCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>()))
            .Returns(new MemberExpression[] { GetLambdaBody(() => testClass.TestParameter) });

        reflectionProviderMock.Setup(p => p.IsParameter(It.IsAny<MemberInfo>())).Returns(true);
        reflectionProviderMock.Setup(p => p.GetValue(It.IsAny<Expression>())).Returns(testClass.TestParameter);
        reflectionProviderMock.Setup(p => p.GetPropertyOrFieldName(It.IsAny<MemberInfo>())).Returns(TestMembersClass.TestParameterName);

        return new MemberExpressionValueCapturer(expressionMembersCapturerMock.Object, reflectionProviderMock.Object, valueCache);
    }

    MemberExpressionValueCapturer BuildTestEvaluationCapturer(TestMembersClass testClass, IValuesCache valueCache)
    {
        expressionMembersCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>()))
            .Returns(new MemberExpression[] { GetLambdaBody(() => testClass.TestParameter) });

        reflectionProviderMock.Setup(p => p.IsParameter(It.IsAny<MemberInfo>())).Returns(false);
        reflectionProviderMock.Setup(p => p.IsEvaluation(It.IsAny<MemberInfo>())).Returns(true);
        reflectionProviderMock.Setup(p => p.GetValue(It.IsAny<Expression>())).Returns(testClass.TestEvaluation);
        reflectionProviderMock.Setup(p => p.GetPropertyOrFieldName(It.IsAny<MemberInfo>())).Returns(TestMembersClass.TestEvaluationName);

        return new MemberExpressionValueCapturer(expressionMembersCapturerMock.Object, reflectionProviderMock.Object, valueCache);
    }

    MemberExpression GetLambdaBody(Expression expression) => (MemberExpression)((LambdaExpression)expression).Body;
}
