using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Evaluation
{
    public static class Constant
    {
        public const string TestEvaluationName = "TEST-EVALUATION-NAME";
        public const string TestEvaluationNameOne = "TEST-EVALUATION-NAME-ONE";
        public const string TestEvaluationNameTwo = "TEST-EVALUATION-NAME-TWO";
    }

    public class EvaluationContextCompostionTests
    {

        [Fact]
        public void Calculation_MixedContexts_ResultAndArgumentsExpected()
        {
            Number result = TestCalculationMixedContexts.Return();
            result.Primitive.Should().Be(20);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_AsPureFunction_ResultAndArgumentsExpected()
        {
            Number result = TestCalcuationPureFunction.Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_ExtensionMethod_ResultAndArgumentsExpected()
        {
            Number result = TestCalcuationExtensionMethod.Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_EncapsulatedContext_ResultAndArgumentsExpected()
        {
            Number result = new TestCalculationEncapsulated().Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_InhertiedFromContext_ResultAndArgumentsExpected()
        {
            Number result = new TestCalculationInherited().Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }
    }

    public class TestCalculationMixedContexts
    {
        public static Number Return()
        {
            EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };

            EvaluationScope<Number>
                CalculationOne = new(options),
                CalculationOther = new(options);

            Number
                NumberOne = Number.Of(2),
                NumberTwo = Number.Of(3),
                NumberThree = Number.Of(5),
                NumberFour = Number.Of(10);

            Number ResultOne = CalculationOne.Evaluate(() => NumberOne + NumberTwo);
            Number ResultOther = CalculationOther.Evaluate(() => NumberThree + NumberFour);

            return CalculationOther.Evaluate(() => ResultOne + ResultOther, Constant.TestEvaluationName);
        }
    }

    public class TestCalcuationPureFunction
    {
        public static Number Return()
        {
            EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
            EvaluationScope<Number> Calculation = new(options);

            Number
                NumberOne = Number.Of(2),
                NumberTwo = Number.Of(3);

            return Calculation.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
        }
    }

    public class TestCalcuationExtensionMethod
    {
        public static Number Return()
        {
            Number
                NumberOne = Number.Of(2),
                NumberTwo = Number.Of(3);

            return Result.Of(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
        }
    }

    public class TestCalculationEncapsulated
    {
        private readonly EvaluationScope<Number> context = new();

        readonly Number
            NumberOne = Number.Of(2),
            NumberTwo = Number.Of(3);

        public Number Return() => context.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
    }

    public class TestCalculationInherited : EvaluationScope<Number>
    {
        readonly Number
            NumberOne = Number.Of(2),
            NumberTwo = Number.Of(3);

        public override Number Return() => Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
    }
}
