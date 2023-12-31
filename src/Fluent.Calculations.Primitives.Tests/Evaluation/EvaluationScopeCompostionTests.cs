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
            Number result = TestCalculationMixedScopes.Return();
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
            string
                expectedScope = string.Concat(GetType().Name, ".", Constant.TestEvaluationScope);

            EvaluationScope scope = Scope.CreateHere(this, Constant.TestEvaluationScope);

            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            Number result = scope.Evaluate(() => NumberOne + NumberTwo, Constant.TestEvaluationName);

            result.Primitive.Should().Be(5);
            result.Name.Should().Be(Constant.TestEvaluationName);
            result.Scope.Should().Be(expectedScope);
            result.Expression.Arguments.Should().HaveCount(2);
            result.Expression.Arguments.First().Scope.Should().Be(expectedScope);
            result.Expression.Arguments.Last().Scope.Should().Be(expectedScope);
        }

        [Fact]
        public void Calculation_WithDependentScope_ResultAndArgumentsExpected()
        {
            string 
                expectedMainScope = string.Concat(typeof(TestCalculationWithDependentScope).Name, ".MAIN-SCOPE"),
                expectedChildScope = string.Concat(typeof(TestCalculationWithDependentScope).Name, ".CHILD-SCOPE"); ;

            Number result = new TestCalculationWithDependentScope().Return();
            result.Primitive.Should().Be(10);
            result.Scope.Should().Be(expectedMainScope);

            var dependentCalculationResult = result.Expression.Arguments.Last();
            dependentCalculationResult.Scope.Should().Be(expectedChildScope);
            dependentCalculationResult.Primitive.Should().Be(5);
            dependentCalculationResult.Expression.Arguments.First().Scope.Should().Be(expectedChildScope);
        }
    }

    public class TestCalculationWithDependentScope
    {
        public Number Return()
        {
            var scope = Scope.CreateHere(this, "MAIN-SCOPE");

            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            return scope.Evaluate(() => NumberOne + NumberTwo + DependencyCalculation(), "MAIN-RESULT");
        }

        public Number DependencyCalculation()
        {
            var scope = Scope.CreateHere(this, "CHILD-SCOPE");

            Number
                A = Number.Of(2, nameof(A)),
                B = Number.Of(3, nameof(B));

            return scope.Evaluate(() => A + B, "CHILD-RESULT");
        }
    }

    public class TestCalculationMixedScopes
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
