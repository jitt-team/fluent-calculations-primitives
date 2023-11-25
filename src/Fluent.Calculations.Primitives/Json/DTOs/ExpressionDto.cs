namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;
using System.Text.Json.Serialization;

[DebuggerDisplay("Body = {Body}")]
internal sealed class ExpressionDto : IExpression
{
    public IArguments Arguments { get; set; } = new ArgumentsDto();

    public string Body { get; set; } = StringConstants.NaN;

    public string Type { get; set; } = StringConstants.NaN;
}
