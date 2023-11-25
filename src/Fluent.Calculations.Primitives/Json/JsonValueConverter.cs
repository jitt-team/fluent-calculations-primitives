namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonValueConverter
{
    public static string ToJson(IValueMetadata value)
    {
        JsonSerializerOptions options = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
        return JsonSerializer.Serialize(value, options);
    }

    public static IValueMetadata ToValue(string json)
    {
        JsonSerializerOptions options = new() { Converters = { new JsonStringEnumConverter() } };
        return JsonSerializer.Deserialize<ValueDto>(Enforce.IsNullOrWhiteSpace(json), options) ??
        throw new ArgumentException("Provided Json can't be desrialized.", nameof(json));
    }
}
