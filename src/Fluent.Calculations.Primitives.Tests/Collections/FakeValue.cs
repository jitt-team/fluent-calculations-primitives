using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    internal class FakeValue : IValueProvider
    {
        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public IExpression Expression { get; set; }

        public ITags Tags { get; set; }

        public ValueOriginType Origin => ValueOriginType.Parameter;

        public string Type => GetType().Name;

        public string PrimitiveString => "VALUE-AS-STRING";

        public IValueProvider MakeDefault() => new FakeValue();

        public IValueProvider MakeOfThisType(MakeValueArgs args) => new FakeValue
        {
            Name = args.Name
        };

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
