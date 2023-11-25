namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;
using System.Text.Json.Serialization;

[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
internal sealed class ValueDto : IValueMetadata
{
    public string Type { get; set; }

    public string Name { get; set; }

    public decimal Primitive { get; set; }

    public ValueOriginType Origin { get; set; }

    [JsonConverter(typeof(JsonConverterInterfaceToClass<IExpression, ExpressionDto>))]
    public IExpression Expression { get; set; }

    [JsonConverter(typeof(JsonConverterInterfaceToClass<ITags, TagsDto>))]
    public ITags Tags { get; set; }

    public string PrimitiveString { get; set; }
}
