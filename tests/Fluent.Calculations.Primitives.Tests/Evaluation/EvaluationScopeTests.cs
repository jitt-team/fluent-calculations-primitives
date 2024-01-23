using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Evaluation
{
    public class EvaluationScopeTests
    {
        private readonly Mock<IValuesCache> _valuesCacheMock = new(MockBehavior.Strict);
        private readonly Mock<IMemberExpressionValueCapturer> _memberCapturerMock = new(MockBehavior.Strict);

        [Fact]
        public void Evaluation_Result_IsExpected()
        {
            var expected = new ExpectedTestValues();
            Number result = RunNonCachedParameterTestCase(expected);
            result.Primitive.Should().Be(expected.PrimitiveValue);
            result.Name.Should().Be(expected.CalculationName);
        }

        [Fact]
        public void Evaluation_ExpressionArguments_AreExpected()
        {
            var expected = new ExpectedTestValues();
            Number result = RunNonCachedParameterTestCase(expected);
            result.Expression.Arguments.Should().HaveCount(2);
            result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expected.NumberOneName));
            result.Expression.Arguments.Should().Contain(a => a.Name.Equals(expected.NumberTwoName));
        }

        [Fact]
        public void Evaluation_EmptyCache_IsCached()
        {
            var inpuexpected = new ExpectedTestValues();
            Number result = RunNonCachedParameterTestCase(inpuexpected);
            _valuesCacheMock.Verify(c => c.ContainsKey(inpuexpected.CalculationName), Times.Once());
            _valuesCacheMock.Verify(c => c.Add(inpuexpected.CalculationName,
                It.Is<IValueProvider>(value => value.Name.Equals(inpuexpected.CalculationName) && value.Primitive.Equals(inpuexpected.PrimitiveValue))),
                Times.Once());
        }

        [Fact]
        public void Evaluation_InCache_UsesCached()
        {
            var expected = new ExpectedTestValues();
            Number result = RunCachedEvaluationParameterTestCase(expected);
            _valuesCacheMock.Verify(c => c.ContainsKey(expected.CalculationName), Times.Once());
            _valuesCacheMock.Verify(c => c.GetByKey(expected.CalculationName), Times.Once());
        }

        [Fact]
        public void Evaluation_EvaluationMemberInCache_UsesCached()
        {
            var expected = new ExpectedTestValues();
            Number result = RunCachedEvaluationMemberTestCase(expected);
            _valuesCacheMock.Verify(c => c.ContainsName(expected.NumberTwoName), Times.Once());
            _valuesCacheMock.Verify(c => c.GetByName(expected.NumberTwoName), Times.Once());
        }

        public Number RunNonCachedParameterTestCase(ExpectedTestValues expected)
        {
            Number
                 NumberOne = Number.Of(5, StringConstants.NaN),
                 NumberTwo = Number.Of(3, StringConstants.NaN);

            CapturedExpressionMembers capturedMembersMock = MockParameterCaptureResult(
                NumberOne, expected.NumberOneName,
                NumberTwo, expected.NumberTwoName);

            EvaluationScope<Number> calculation = MockAndBuildNonCachedCalculation(expected.CalculationName, capturedMembersMock);

            return calculation.Evaluate(() => NumberOne * NumberTwo, expected.CalculationName);
        }


        public Number RunCachedEvaluationParameterTestCase(ExpectedTestValues expected)
        {
            Number
                 NumberOne = Number.Of(5, StringConstants.NaN),
                 NumberTwo = Number.Of(3, StringConstants.NaN);

            Number
                CachedResult = Number.Of(expected.PrimitiveValue, expected.CalculationName);

            EvaluationScope<Number> calculation = MockAndBuildCachedCalculation(expected.CalculationName, CachedResult);

            return calculation.Evaluate(() => NumberOne * NumberTwo, expected.CalculationName);
        }

        public Number RunCachedEvaluationMemberTestCase(ExpectedTestValues expected)
        {
            Number
                 NumberOne = Number.Of(5, StringConstants.NaN),
                 NumberTwo = Number.Of(3, expected.NumberTwoName);

            CapturedExpressionMembers capturedMembersMock = MockEvaluationCaptureResult(expected.NumberTwoName);

            EvaluationScope<Number> calculation = MockAndBuildCachedParameterCalculation(expected.NumberTwoName, NumberTwo, capturedMembersMock);

            return calculation.Evaluate(() => NumberOne * NumberTwo, expected.CalculationName);
        }

        private EvaluationScope<Number> MockAndBuildNonCachedCalculation(string expectedCalculationName, CapturedExpressionMembers capturedMembersMock)
        {
            _valuesCacheMock.Setup(c => c.ContainsKey(expectedCalculationName)).Returns(false).Verifiable();
            _valuesCacheMock.Setup(c => c.Add(expectedCalculationName, It.IsAny<IValueProvider>())).Verifiable();
            _memberCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>())).Returns(capturedMembersMock).Verifiable();

            EvaluationScope<Number> calculation = new(_valuesCacheMock.Object, _memberCapturerMock.Object);
            return calculation;
        }
        private EvaluationScope<Number> MockAndBuildCachedCalculation(string expectedCalculationName, Number cachedResult)
        {
            _valuesCacheMock.Setup(c => c.ContainsKey(expectedCalculationName)).Returns(true).Verifiable();
            _valuesCacheMock.Setup(c => c.GetByKey(expectedCalculationName)).Returns(cachedResult).Verifiable();

            EvaluationScope<Number> calculation = new(_valuesCacheMock.Object, _memberCapturerMock.Object);
            return calculation;
        }

        private EvaluationScope<Number> MockAndBuildCachedParameterCalculation(string cachedValueName, Number cachedValue, CapturedExpressionMembers capturedMembersMock)
        {
            _valuesCacheMock.Setup(c => c.ContainsKey(It.IsAny<string>())).Returns(false).Verifiable();
            _valuesCacheMock.Setup(c => c.Add(It.IsAny<string>(), It.IsAny<IValueProvider>())).Verifiable();
            _valuesCacheMock.Setup(c => c.ContainsName(cachedValueName)).Returns(true).Verifiable();
            _valuesCacheMock.Setup(c => c.GetByName(cachedValueName)).Returns(cachedValue).Verifiable();
            _memberCapturerMock.Setup(c => c.Capture(It.IsAny<Expression<Func<Number>>>())).Returns(capturedMembersMock).Verifiable();

            EvaluationScope<Number> calculation = new(_valuesCacheMock.Object, _memberCapturerMock.Object);
            return calculation;
        }

        private static CapturedExpressionMembers MockParameterCaptureResult(IValueProvider value1, string name1, IValueProvider value2, string name2)
        {
            var parameterMemberers = new[] {
                new CapturedParameterMember(value1, name1),
                new CapturedParameterMember(value2, name2)
            };

            var evaluationMembers = Array.Empty<CapturedEvaluationMember>();

            return new CapturedExpressionMembers(parameterMemberers, evaluationMembers);
        }

        private static CapturedExpressionMembers MockEvaluationCaptureResult(string name)
        {
            var parameterMemberers = Array.Empty<CapturedParameterMember>();

            var evaluationMembers = new[] { new CapturedEvaluationMember(name) };

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