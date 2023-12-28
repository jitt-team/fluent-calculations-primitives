namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Text.Json.Serialization;

/// <include file="Docs.xml" path='*/IValue/interface/*'/>
public interface IValue
{
    /// <include file="Docs.xml" path='*/IValue/Type/*'/>
    [JsonPropertyName("Type")]
    string Type { get; }

    /// <include file="Docs.xml" path='*/IValue/Name/*'/>
    [JsonPropertyName("Name")]
    string Name { get; }

    /// <include file="Docs.xml" path='*/IValue/Scope/*'/>
    [JsonPropertyName("Scope")]
    string Scope { get; }

    /// <include file="Docs.xml" path='*/IValue/Primitive/*'/>
    [JsonPropertyName("Primitive")]
    decimal Primitive { get; }

    /// <include file="Docs.xml" path='*/IValue/PrimitiveString/*'/>
    [JsonPropertyName("PrimitiveString")]
    string PrimitiveString { get; }

    /// <include file="Docs.xml" path='*/IValue/Origin/*'/>
    [JsonPropertyName("Origin")]
    ValueOriginType Origin { get; }

    /// <include file="Docs.xml" path='*/IValue/Expression/*'/>
    [JsonPropertyName("Expression")]
    IExpression Expression { get; }

    /// <include file="Docs.xml" path='*/IValue/Tags/*'/>
    [JsonPropertyName("Tags")]
    ITags Tags { get; }
}