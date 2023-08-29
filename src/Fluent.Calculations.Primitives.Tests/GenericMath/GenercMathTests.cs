using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
using System.Numerics;

namespace Fluent.Calculations.Primitives.Tests.GenericMath
{
    public class GenercMathTests
    {
        [Fact]
        public void TestDecimal()
        {
            decimal
                One = 1,
                Two = 2,
                Three = 3;

            decimal sum = GenericMathExamples.GenericCalculation(One, Two, Three);

            sum.Should().Be(7m);
        }

        [Fact]
        public void TestNumber()
        {
            var ctx = new EvaluationContext<Number>();

            Number
                One = Number.Of(1),
                Two = Number.Of(2),
                Three = Number.Of(3);

            Number sum = ctx.Evaluate(() => GenericMathExamples.GenericCalculation(One, Two, Three));

            sum.Should().Be(Number.Of(6m));
        }
    }

    public class GenericMathExamples
    {
        public static TNumber GenericCalculation<TNumber>(TNumber valueOne, TNumber valueTwo, TNumber valueThree)
            where TNumber :
                IAdditionOperators<TNumber, TNumber, TNumber>,
                IMultiplyOperators<TNumber, TNumber, TNumber>
        {

            TNumber result = valueOne + valueTwo * valueThree;
            return result;
        }
    }
}
