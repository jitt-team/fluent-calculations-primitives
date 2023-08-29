using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Tests.Composition;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fluent.Calculations.Primitives.Tests.GenericMath
{
    public class GenercMathTests
    {
        [Fact]
        public void TestDecimal()
        {
            //decimal
            //    One = 1,
            //    Two = 2,
            //    Three = 3;

            //bool Condition = true;

            //decimal sum = GenericMathExamples.GenericCalculation(One, Two, Three, Condition);

            //sum.Should().Be(7m);
        }

        [Fact]
        public void TestNumber()
        {
            var ctx = new EvaluationContext<Number>();

            Number
                One = Number.Of(1),
                Two = Number.Of(2),
                Three = Number.Of(3);

            Condition Condition = Condition.True();

            Number sum = ctx.Evaluate(() => GenericMathExamples.GenericCalculation(One, Two, Three, Condition));

            sum.Should().Be(Number.Of(6m));
        }
    }

    public class GenericMathExamples
    {
        public static TNumber GenericCalculation<TNumber, TCondition>(TNumber valueOne, TNumber valueTwo, TNumber valueThree, TCondition condition)
            where TNumber :
                IAdditionOperators<TNumber, TNumber, TNumber>,
                IMultiplyOperators<TNumber, TNumber, TNumber>
            where TCondition :
                IEqualityOperators<TCondition, TCondition, TCondition>,
                IBitwiseOperators<TCondition, TCondition, TCondition>
        {

            TNumber result = valueOne + valueTwo * valueThree;

            return result;
        }
    }
}
