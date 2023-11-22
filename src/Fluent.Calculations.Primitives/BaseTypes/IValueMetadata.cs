namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Text.Json.Serialization;

public interface IValueMetadata
{
    [JsonPropertyName("Type")]
    string Type { get; }

    [JsonPropertyName("Name")]
    string Name { get; }

    [JsonPropertyName("Primitive")]
    decimal Primitive { get; }

    [JsonPropertyName("PrimitiveString")]
    string PrimitiveString { get; }

    [JsonPropertyName("Origin")]
    ValueOriginType Origin { get; }

    [JsonPropertyName("Expression")]
    ExpressionNode Expression { get; }

    [JsonPropertyName("Tags")]
    TagsCollection Tags { get; }


}