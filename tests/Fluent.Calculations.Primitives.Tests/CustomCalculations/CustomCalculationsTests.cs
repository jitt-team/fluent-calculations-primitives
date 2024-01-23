using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
using System.Numerics;

namespace Fluent.Calculations.Primitives.Tests.CustomMath
{
    public class CustomCalculationsTests
    {
        [Fact]
        public void Calculate_CustomFunction_EvaluationNumber_ResultExpected()
        {
            decimal One = 1, Two = 2, Three = 3;

            decimal result = CustomGenericCalculation.CalculateTestFormula(One, Two, Three);

            result.Should().Be(7m);
        }

        [Fact]
        public void Calculate_CustomFunction_Decimal_ResultExpected()
        {
            EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
            EvaluationScope<Number> context = new(options);
            Number One = 1, Two = 2, Three = 3;

            Number result = context.Evaluate(() => CustomGenericCalculation.CalculateTestFormula(One, Two, Three));

            result.Should().Be(Number.Of(7m));
            result.Expression.Body.Should().Be("CustomGenericCalculation.CalculateTestFormula(One, Two, Three)");
            result.Expression.Arguments.Should().HaveCount(3);
        }
    }

    public class CustomGenericCalculation
    {
        public static TNumber CalculateTestFormula<TNumber>(TNumber valueOne, TNumber valueTwo, TNumber valueThree)
            where TNumber :
                IAdditionOperators<TNumber, TNumber, TNumber>,
                IMultiplyOperators<TNumber, TNumber, TNumber>
            => valueOne + (valueTwo * valueThree);
    }
}
