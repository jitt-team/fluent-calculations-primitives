
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class CollectionExpressionHandlerTests
    {
        [Fact]
        public void Test()
        {
            Number
                NumberOne = Number.Of(2, nameof(NumberOne)),
                NumberTwo = Number.Of(3, nameof(NumberTwo));

            Values<Number> MultipleNumbers = Values<Number>.Of(() => new[] { NumberOne, NumberTwo }, nameof(MultipleNumbers));


        }
    }
}
