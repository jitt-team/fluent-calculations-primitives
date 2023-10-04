namespace Fluent.Calculations.Primitives.Tests.Evaluation;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using Moq;

public class EvaluationContextTests
{
    private IMock<IValuesCache> _valuesCacheMock;
    private IMock<IMemberExpressionValueCapturer> _memberCapturerMock;
    [Fact]
    public void Test()
    {
        Number
            NumberOne = Number.Of(5, "TEST-NUMBER-ONE"),
            NumberTwo = Number.Of(3, "TEST-NUMBER-TWO");

        var calculation = BuildCalculation();



        Number result = calculation.Evaluate(() => NumberOne * NumberTwo);
    }

    private EvaluationContext<Number> BuildCalculation()
    {
        _valuesCacheMock = new Mock<IValuesCache>(MockBehavior.Strict);
        _memberCapturerMock = new Mock<IMemberExpressionValueCapturer>(MockBehavior.Strict);

        EvaluationContext<Number> calculation = new EvaluationContext<Number>(_valuesCacheMock.Object, _memberCapturerMock.Object);
        return calculation;
    }
}
