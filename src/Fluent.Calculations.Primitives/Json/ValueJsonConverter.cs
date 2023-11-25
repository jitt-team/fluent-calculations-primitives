namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public static class ValueJsonConverter
{
    public static string ToJson(IValue value)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Converters = {
                new JsonStringEnumConverter() },
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { IgnoreEpmptyTags }
            }
        };
        return JsonSerializer.Serialize(value, options);
    }

    public static IValue ToValue(string json)
    {
        JsonSerializerOptions options = new()
        {
            Converters = {
                new JsonStringEnumConverter(),
                new JsonConverterInterfaceToClass<IExpression, ExpressionDto>(),
                new JsonConverterInterfaceToClass<ITags, TagsDto>(),
                new JsonArgumentsReader()
            }
        };
        return JsonSerializer.Deserialize<ValueDto>(Enforce.IsNullOrWhiteSpace(json), options) ??
        throw new ArgumentException("Provided Json string can not be deserialized.", nameof(json));
    }

    private static void IgnoreEpmptyTags(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type != typeof(IValue))
            return;

        foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
        {
            if (propertyInfo.PropertyType == typeof(ITags))
            {
                propertyInfo.ShouldSerialize = static (obj, value) =>
                    value != null && ((ITags)value).Any();
            }
        }
    }
}
