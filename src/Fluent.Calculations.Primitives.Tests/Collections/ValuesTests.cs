namespace Fluent.Calculations.Primitives.Tests.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class ValuesTests
{
    [Fact]
    public void MakeOfThisElementType_CreatesNewInstance_OfElementType()
    {
        string expectedName = "EXPECTED-NAME";
        Values<FakeValue> values = new();
        IValue element = values.MakeOfThisElementType(MakeValueArgs.Compose(expectedName, ExpressionNode.None, 0));
        element.Should().NotBeNull();
        element.Name.Should().Be(expectedName);
    }

    [Fact]
    public void WithMultipleElements_AccessibleThroughIEnumerable()
    {
        Values<FakeValue> values = Values<FakeValue>.SumOf(() => new FakeValue[] {
            new() { Name = "ITEM-1", Primitive = 1 },
            new() { Name = "ITEM-2", Primitive = 2 } });

        values.Count.Should().Be(2);
        values.Primitive.Should().Be(3);
        values.First().Name.Should().Be("ITEM-1");
        values.Last().Name.Should().Be("ITEM-2");
    }
}
