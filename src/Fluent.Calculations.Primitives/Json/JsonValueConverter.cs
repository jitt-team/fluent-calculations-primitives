namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Utils;
using System.Text.Json;

public static class JsonValueConverter
{
    public static string ToJson(IValueMetadata value) => JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });

    public static IValueMetadata ToValue(string json)
    {
        return JsonSerializer.Deserialize<ValueDto>(Enforce.IsNullOrWhiteSpace(json)) ??
        throw new ArgumentException("Provided Json can't be desrialized.", nameof(json));
    }
}
