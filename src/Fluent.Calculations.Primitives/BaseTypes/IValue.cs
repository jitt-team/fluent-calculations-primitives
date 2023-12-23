namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Text.Json.Serialization;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="IValue"]/class/*' />
public interface IValue
{
    [JsonPropertyName("Type")]
    string Type { get; }

    [JsonPropertyName("Name")]
    string Name { get; }

    [JsonPropertyName("Scope")]
    string Scope { get; }

    [JsonPropertyName("Primitive")]
    decimal Primitive { get; }

    [JsonPropertyName("PrimitiveString")]
    string PrimitiveString { get; }

    [JsonPropertyName("Origin")]
    ValueOriginType Origin { get; }

    [JsonPropertyName("Expression")]
    IExpression Expression { get; }

    [JsonPropertyName("Tags")]
    ITags Tags { get; }
}