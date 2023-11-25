namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class ValueJsonConverter
{
    public static string ToJson(IValue value)
    {
        JsonSerializerOptions options = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
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
                new JsonConverterArguments(),
            }
        };
        return JsonSerializer.Deserialize<ValueDto>(Enforce.IsNullOrWhiteSpace(json), options) ??
        throw new ArgumentException("Provided Json can't be desrialized.", nameof(json));
    }
}
