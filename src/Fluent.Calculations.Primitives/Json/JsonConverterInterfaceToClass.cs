namespace Fluent.Calculations.Primitives.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class JsonConverterInterfaceToClass<TSourceInteface, TTargetClass> : JsonConverter<TSourceInteface> where TTargetClass : class, TSourceInteface
{
    public override TSourceInteface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => JsonSerializer.Deserialize<TTargetClass>(ref reader);

    public override void Write(Utf8JsonWriter writer, TSourceInteface value, JsonSerializerOptions options) { }
}
