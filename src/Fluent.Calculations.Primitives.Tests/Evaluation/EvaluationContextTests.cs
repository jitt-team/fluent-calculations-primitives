namespace Fluent.Calculations.Primitives.Tests.Evaluation;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
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
            expectedValue1Name = "NUMBER-1",
            expectedValue2Name = "NUMBER-2";

        Number
            NumberOne = Number.Of(5, "NONAME-1"),
            NumberTwo = Number.Of(3, "NONAME-2");

        CapturedExpressionMembers capturedMembersMock = MockOnlyParameterCaptureResult(
            NumberTwo, expectedValue1Name,
            NumberTwo, expectedValue2Name);

        var calculation = MockAndBuildCalculation(expectedCalculationName, capturedMembersMock);

        Number result = calculation.Evaluate(() => NumberOne * NumberTwo, expectedCalculationName);

        _valuesCacheMock.Verify(c => c.ContainsKey(expectedCalculationName), Times.Once());
        _valuesCacheMock.Verify(c => c.Add(expectedCalculationName, It.IsAny<IValue>()), Times.Once());

    }

    private EvaluationContext<Number> MockAndBuildCalculation(string expectedCalculationName, CapturedExpressionMembers capturedMembersMock)
    {
        _valuesCacheMock.Setup(c => c.ContainsKey(expectedCalculationName)).Returns(false);
        _valuesCacheMock.Setup(c => c.Add(expectedCalculationName, It.IsAny<IValue>()));
        _memberCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>())).Returns(capturedMembersMock);
        
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
