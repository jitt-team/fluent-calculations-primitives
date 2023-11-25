using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class CollectionExpressionHandlerTests
    {
        [Fact]
        public void AggregateOperation_CollectionOfTwo_IsExpectedResult()
        {
            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            Values<Number> MultipleNumbers = Values<Number>.SumOf(() => new[] { NumberOne, NumberTwo }, nameof(MultipleNumbers));

            Number result = CollectionExpressionHandler.Handle(MultipleNumbers, TestAggregateMethod, "TestAggregateMethod");

            result.Primitive.Should().Be(5);
            result.Name.Should().Be("TestAggregateMethod");
            result.Expression.Body.Should().Be("TestAggregateMethod(MultipleNumbers)");
            result.Expression.Arguments.Count.Should().Be(1);
            IValue multipleNumbersArgument = result.Expression.Arguments.First();
            multipleNumbersArgument.Name.Should().Be("MultipleNumbers");
            multipleNumbersArgument.Expression.Arguments.First().Name.Should().Be("NumberOne");
            multipleNumbersArgument.Expression.Arguments.Last().Name.Should().Be("NumberTwo");
        }

        public static decimal TestAggregateMethod<TSource>(IEnumerable<TSource> source, Func<TSource, decimal> selector) => 5.00m;
    }
}
