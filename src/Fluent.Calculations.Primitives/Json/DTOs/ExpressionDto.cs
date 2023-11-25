namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;
using System.Text.Json.Serialization;

[DebuggerDisplay("Body = {Body}")]
public class ExpressionDto : IExpression
{
    [JsonConverter(typeof(JsonConverterInterfaceToClass<IArguments, ArgumentsDto>))]
    public IArguments Arguments { get; set; }

    public string Body { get; set; }

    public string Type { get; set; }
}
