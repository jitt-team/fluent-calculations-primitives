using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;

namespace Fluent.Calculations.Primitives.BaseTypes
{
    public class ValueJson : IValueMetadata
    {
        public static IValueMetadata FromJson(string json) => JsonSerializer.Deserialize<ValueJson>(Enforce.IsNullOrWhiteSpace(json)) ??
            throw new ArgumentException("Provided Json can't be desrialized.", nameof(json));

        public string Type { get; set; }

        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public ValueOriginType Origin { get; set; }

        public ExpressionNode Expression { get; set; }

        public TagsCollection Tags { get; set; }

        public string PrimitiveString { get; set; }
    }
}
