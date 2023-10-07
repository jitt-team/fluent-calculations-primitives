using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
using System.Numerics;

namespace Fluent.Calculations.Primitives.Tests.CustomMath
{
    public class CustomCalculationsTests
    {
        [Fact]
        public void TestDecimal()
        {
            decimal One = 1, Two = 2, Three = 3;
            decimal sum = CustomGenericCalculation.CalculateTestFormula(One, Two, Three);
            sum.Should().Be(7m);
        }

        [Fact(Skip = "Skip until implementing methid call capture")]
        public void TestNumber()
        {
            var context = new EvaluationContext<Number>();

            Number One = 1, Two = 2, Three = 2;
            Number sum = context.Evaluate(() => CustomGenericCalculation.CalculateTestFormula(One, Two, Three));
            sum.Should().Be(Number.Of(7m));
        }
    }

    public class CustomGenericCalculation
    {
        public static TNumber CalculateTestFormula<TNumber>(TNumber valueOne, TNumber valueTwo, TNumber valueThree)
            where TNumber :
                IAdditionOperators<TNumber, TNumber, TNumber>,
                IMultiplyOperators<TNumber, TNumber, TNumber>
            => valueOne + valueTwo * valueThree;
    }
}
