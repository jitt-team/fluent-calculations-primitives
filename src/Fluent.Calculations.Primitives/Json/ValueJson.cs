using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluent.Calculations.Primitives.Json
{
    public class ValueJson : IValueMetadata
    {
        public static IValueMetadata FromJson(string json)
        {
            var options = new JsonSerializerOptions();
            return JsonSerializer.Deserialize<ValueJson>(Enforce.IsNullOrWhiteSpace(json), options) ??
            throw new ArgumentException("Provided Json can't be desrialized.", nameof(json));
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public ValueOriginType Origin { get; set; }

        public IExpression Expression { get; set; }

        public ITags Tags { get; set; }

        public string PrimitiveString { get; set; }
    }
}
