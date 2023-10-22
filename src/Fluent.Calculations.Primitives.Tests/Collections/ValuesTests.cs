using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    public class ValuesTests
    {
        [Fact]
        public void Test()
        {
            var values = new Values<MyValueType>();
            var element = values.MakeOfThisElementType(MakeValueArgs.Compose("EXPECTED-NAME", ExpressionNode.None, 0));
            element.Should().NotBeNull();
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
