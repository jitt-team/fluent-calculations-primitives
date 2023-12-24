namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class JsonArgumentsReader : JsonConverter<IArguments>
{
    public override IArguments Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<ValueDto>? argumentsList = JsonSerializer.Deserialize<List<ValueDto>>(ref reader, options);
        ArgumentsDto arguments = [.. argumentsList ?? []];
        return arguments;
    }

    public override void Write(Utf8JsonWriter writer, IArguments value, JsonSerializerOptions options) { }
}