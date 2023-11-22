using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    internal class FakeValue : IValue
    {
        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public ExpressionNode Expression { get; set; }

        public TagsCollection Tags { get; set; }

        public ValueOriginType Origin => ValueOriginType.Parameter;

        public string Type => GetType().Name;

        public string PrimitiveString => "VALUE-AS-STRING";

        public IValue GetDefault() => new FakeValue();

        public IValue MakeOfThisType(MakeValueArgs args) => new FakeValue
        {
            Name = args.Name
        };

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
