using Fluent.Calculations.Primitives;
using FluentAssertions;

namespace Fluent.Calculations.Tests.Calculation
{
    public class CalculationTests
    {
        [Fact]
        public void Test()
        {
            var calculation = new FooBarCalculation
            {
                Foo = Number.Of(2),
                Bar = Number.Of(3)
            };

            Number result = calculation.Calculate();

            result.Should().NotBeNull();
        }
    }

    internal class FooBarCalculation : Calculation<Number>
    {
        public Number
            Foo = Number.Zero,
            Bar = Number.Zero;

        private Number FooBar => Is(() => Foo + Bar);

        private Number FooBarTotal => Is(() => Foo + FooBar);

        public override Number Return() => FooBarTotal;
    }
}
