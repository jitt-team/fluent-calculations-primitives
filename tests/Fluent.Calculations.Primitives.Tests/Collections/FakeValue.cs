using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    internal class FakeValue : IValueProvider
    {
        public string Name { get; set; } = StringConstants.NaN;

        public decimal Primitive { get; set; }

        public IExpression Expression { get; set; } = ExpressionNode.None;

        public ITags Tags { get; set; } = TagsCollection.Empty;

        public ValueOriginType Origin => ValueOriginType.Parameter;

        public string Type => GetType().Name;

        public string Scope => StringConstants.NaN;

        public string PrimitiveString => "VALUE-AS-STRING";

        public IValue Accept(ValueVisitor visitor) => ArgumentsVisitorInvoker.VisitArguments(this, visitor);

        public IValueProvider MakeDefault() => new FakeValue();

        public IValueProvider MakeOfThisType(MakeValueArgs args) => new FakeValue
        {
            Name = args.Name
        };
    }
}
