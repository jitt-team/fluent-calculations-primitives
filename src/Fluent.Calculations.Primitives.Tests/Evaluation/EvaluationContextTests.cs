
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Evaluation
{
    public class EvaluationContextTests
    {
        private Mock<IValuesCache> _valuesCacheMock = new Mock<IValuesCache>(MockBehavior.Strict);
        private Mock<IMemberExpressionValueCapturer> _memberCapturerMock = new Mock<IMemberExpressionValueCapturer>(MockBehavior.Strict);

        [Fact]
        public void Evaluation_Result_IsExpected()
        {
            var expected = new ExpectedTestValues();
            Number result = RunParameterTestCase(expected);
            result.Primitive.Should().Be(expected.PrimitiveValue);
            result.Name.Should().Be(expected.CalculationName);
        }

        [Fact]
        public void Evalueation_ExpressionArguments_AreExpected()
        {
            var expected = new ExpectedTestValues();
            Number result = RunParameterTestCase(expected);
            result.Expression.Arguments.Should().HaveCount(2);
            result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expected.NumberOneName));
            result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expected.NumberTwoName));
        }

        [Fact]
        public void Evaluation_EmptyCache_IsCached()
        {
            var inpuexpected = new ExpectedTestValues();
            Number result = RunParameterTestCase(inpuexpected);
            _valuesCacheMock.Verify(c => c.ContainsKey(inpuexpected.CalculationName), Times.Once());
            _valuesCacheMock.Verify(c => c.Add(inpuexpected.CalculationName,
                It.Is<IValue>(value => value.Name.Equals(inpuexpected.CalculationName) && value.Primitive.Equals(inpuexpected.PrimitiveValue))),
                Times.Once());
        }

        public Number RunParameterTestCase(ExpectedTestValues expected)
        {
            Number
                 NumberOne = Number.Of(5, Constants.NaN),
                 NumberTwo = Number.Of(3, Constants.NaN);

            CapturedExpressionMembers capturedMembersMock = MockParameterCaptureResult(
                NumberOne, expected.NumberOneName,
                NumberTwo, expected.NumberTwoName);

            EvaluationContext<Number> calculation = MockAndBuildCalculation(expected.CalculationName, capturedMembersMock);

            return calculation.Evaluate(() => NumberOne * NumberTwo, expected.CalculationName);
        }

        private EvaluationContext<Number> MockAndBuildCalculation(string expectedCalculationName, CapturedExpressionMembers capturedMembersMock)
        {
            _valuesCacheMock.Setup(c => c.ContainsKey(expectedCalculationName)).Returns(false).Verifiable();
            _valuesCacheMock.Setup(c => c.Add(expectedCalculationName, It.IsAny<IValue>())).Verifiable();
            _memberCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>())).Returns(capturedMembersMock).Verifiable();

            EvaluationContext<Number> calculation = new EvaluationContext<Number>(_valuesCacheMock.Object, _memberCapturerMock.Object);
            return calculation;
        }

        private CapturedExpressionMembers MockParameterCaptureResult(IValue value1, string name1, IValue value2, string name2)
        {
            var parameterMemberers = new[] {
            new CapturedParameterMember(value1, name1),
            new CapturedParameterMember(value2, name2)
        };
            var evaluationMembers = new CapturedEvaluationMember[0];

            return new CapturedExpressionMembers(parameterMemberers, evaluationMembers);
        }

        public class ExpectedTestValues
        {
            public string
                CalculationName = "TEST-CALCULATION",
                NumberOneName = "NUMBER-1",
                NumberTwoName = "NUMBER-2";

            public decimal PrimitiveValue = 15m;
        }
    }
}