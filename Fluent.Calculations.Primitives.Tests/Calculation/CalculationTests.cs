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

        Condition ByBoo = Condition.True(); //> Is(() => Foo > Bar);

        Number FooBar => Is(() => ByBoo ? Foo : Bar);

        // Number FooBarTotal => Is(() => Foo + FooBar);

        public override Number Return() => FooBar;
    }
}
