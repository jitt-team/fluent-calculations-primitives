namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public static class ValueJsonConverter
{
    public static string Serialize(IValue value, bool writeIndented = true)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = writeIndented,
            Converters = {
                new JsonStringEnumConverter() },
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = {
                    IgnoreEpmptyList<IValue, ITags, Tag>,
                    IgnoreEpmptyList<IExpression, IArguments, IValue>
                }
            }
        };
        return JsonSerializer.Serialize(value, options);
    }

    public static IValue Deserialize(string json)
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

    private static void IgnoreEpmptyList<TContainingType, TListType, IElementType>(JsonTypeInfo typeInfo) where TListType : IEnumerable<IElementType>
    {
        if (typeInfo.Type != typeof(TContainingType))
            return;

        var listProperties = typeInfo.Properties.Where(p => p.PropertyType == typeof(TListType));

        foreach (JsonPropertyInfo propertyInfo in listProperties)
            propertyInfo.ShouldSerialize = static (obj, value) =>
                value != null && ((TListType)value).Any();
    }
}
