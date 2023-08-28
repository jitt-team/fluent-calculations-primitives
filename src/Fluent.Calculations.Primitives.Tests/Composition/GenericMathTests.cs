using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fluent.Calculations.Primitives.Tests.Composition
{
    public class GenericMathTests
    {
        [Fact]
        public void Test()
        {
            var ctx = new EvaluationContext<Number>();

            decimal sum = GenericMathExamples.GeneticAddition(2m, 4m);
            Number 
                One = Number.Of(2),
                Two = Number.Of(4);

            Number sumNum = ctx.Evaluate(() => GenericMathExamples.GeneticAddition(One, Two));
            sum.Should().Be(sumNum.PrimitiveValue);
        }
    }

    public class GenericMathExamples
    {
        public static T GeneticAddition<T>(T a, T b) where T : IAdditionOperators<T, T, T>
             => a + b;

    }
}
