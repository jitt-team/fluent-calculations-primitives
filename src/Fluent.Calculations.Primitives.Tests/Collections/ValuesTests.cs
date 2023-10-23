using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    public class ValuesTests
    {
        [Fact]
        public void MakeOfThisElementType_CreatesNewInstance_OfElementType()
        {
            string expectedName = "EXPECTED-NAME";
            Values<MyValueType> values = new Values<MyValueType>();
            IValue element = values.MakeOfThisElementType(MakeValueArgs.Compose(expectedName, ExpressionNode.None, 0));
            element.Should().NotBeNull();
            element.Name.Should().Be(expectedName);
        }

        [Fact]
        public void WithMultipleElements_AccessibleThroughIEnumerable()
        {
            Values<MyValueType> values = Values<MyValueType>.SumOf(() => new MyValueType[] {
                new() { Name = "ITEM-1", Primitive = 1 },
                new() { Name = "ITEM-2", Primitive = 2 } });

            values.Count.Should().Be(2);
            values.Primitive.Should().Be(3);
            values.First().Name.Should().Be("ITEM-1");
            values.Last().Name.Should().Be("ITEM-2");
        }
    }

    internal class MyValueType : IValue
    {
        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public bool IsParameter { get; set; }

        public bool IsOutput { get; set; }

        public ExpressionNode Expression { get; set; }

        public TagsCollection Tags { get; set; }

        public IValue Default => new MyValueType();

        public IValue MakeOfThisType(MakeValueArgs args) => new MyValueType
        {
            Name = args.Name
        };

        public string ValueToString() => "VALUE-AS-STRING";
    }
}
