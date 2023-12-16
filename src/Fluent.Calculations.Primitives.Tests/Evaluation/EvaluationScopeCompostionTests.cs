using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Evaluation
{
    public static class Constant
    {
        public const string TestEvaluationName = "TEST-EVALUATION-NAME";
        public const string TestEvaluationNameOne = "TEST-EVALUATION-NAME-ONE";
        public const string TestEvaluationNameTwo = "TEST-EVALUATION-NAME-TWO";
        public const string TestEvaluationScope = "TEST-EVALUATION-SCOPE";
    }

    public class EvaluationScopeCompostionTests
    {

        [Fact]
        public void Calculation_MixedScopes_ResultAndArgumentsExpected()
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
        public void Calcuation_EncapsulatedScope_ResultAndArgumentsExpected()
        {
            Number result = new TestCalculationEncapsulated().Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_InhertiedFromScope_ResultAndArgumentsExpected()
        {
            Number result = new TestCalculationInherited().Return();
            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Expression.Arguments.Should().HaveCount(2);
        }

        [Fact]
        public void Calcuation_LocalScope_ResultAndArgumentsExpected()
        {
            EvaluationScope scope = this.GetScope(Constant.TestEvaluationScope);

            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            Number result = scope.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);

            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Scope.Should().Be(Constant.TestEvaluationScope);
            result.Expression.Arguments.Should().HaveCount(2);
            result.Expression.Arguments.First().Scope.Should().Be(Constant.TestEvaluationScope);
            result.Expression.Arguments.Last().Scope.Should().Be(Constant.TestEvaluationScope);
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
            EvaluationScope<Number> scope = new(options);

            Number
                NumberOne = Number.Of(2),
                NumberTwo = Number.Of(3);

            return scope.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
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
        private readonly EvaluationScope<Number> scope = new();

        readonly Number
            NumberOne = Number.Of(2),
            NumberTwo = Number.Of(3);

        public Number Return() => scope.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
    }

    public class TestCalculationInherited : EvaluationScope<Number>
    {
        readonly Number
            NumberOne = Number.Of(2),
            NumberTwo = Number.Of(3);

        public override Number Return() => Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);
    }
}
