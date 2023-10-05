namespace Fluent.Calculations.Primitives.Tests.Evaluation;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

public class EvaluationContextTests
{
    private Mock<IValuesCache> _valuesCacheMock = new Mock<IValuesCache>(MockBehavior.Strict);
    private Mock<IMemberExpressionValueCapturer> _memberCapturerMock = new Mock<IMemberExpressionValueCapturer>(MockBehavior.Strict);

    [Fact]
    public void Test()
    {
        string
            expectedCalculationName = "TEST-CALCULATION",
            expectedNumberOneName = "NUMBER-1",
            expectedNumberTwoName = "NUMBER-2";
      
        Number
             NumberOne = Number.Of(5, Constants.NaN),
             NumberTwo = Number.Of(3, Constants.NaN);

        CapturedExpressionMembers capturedMembersMock = MockOnlyParameterCaptureResult(
            NumberOne, expectedNumberOneName,
            NumberTwo, expectedNumberTwoName);

        var calculation = MockAndBuildCalculation(expectedCalculationName, capturedMembersMock);

        Number result = calculation.Evaluate(() => NumberOne * NumberTwo, expectedCalculationName);

        result.Primitive.Should().Be(15m);
        result.Name.Should().Be(expectedCalculationName);
        result.Expression.Arguments.Should().HaveCount(2);
        result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expectedNumberOneName));
        result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expectedNumberTwoName));
        _valuesCacheMock.Verify(c => c.ContainsKey(expectedCalculationName), Times.Once());
        _valuesCacheMock.Verify(c => c.Add(expectedCalculationName,
            It.Is<IValue>(value => value.Name.Equals(expectedCalculationName) && value.Primitive.Equals(15m))),
            Times.Once());
    }

    private EvaluationContext<Number> MockAndBuildCalculation(string expectedCalculationName, CapturedExpressionMembers capturedMembersMock)
    {
        _valuesCacheMock.Setup(c => c.ContainsKey(expectedCalculationName)).Returns(false).Verifiable();
        _valuesCacheMock.Setup(c => c.Add(expectedCalculationName, It.IsAny<IValue>())).Verifiable();
        _memberCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>())).Returns(capturedMembersMock).Verifiable();

        EvaluationContext<Number> calculation = new EvaluationContext<Number>(_valuesCacheMock.Object, _memberCapturerMock.Object);
        return calculation;
    }

    private CapturedExpressionMembers MockOnlyParameterCaptureResult(IValue value1, string name1, IValue value2, string name2)
    {
        var parameterMemberers = new[] {
            new CapturedParameterMember(value1, name1),
            new CapturedParameterMember(value2, name2)
        };
        var evaluationMembers = new CapturedEvaluationMember[0];

        return new CapturedExpressionMembers(parameterMemberers, evaluationMembers);
    }
}
