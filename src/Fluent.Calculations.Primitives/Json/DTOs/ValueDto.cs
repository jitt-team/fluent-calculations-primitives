namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;

[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
internal sealed class ValueDto : IValue
{
    public string Type { get; set; } = StringConstants.NaN;

    public string Name { get; set; } = StringConstants.NaN;

    public decimal Primitive { get; set; }

    public ValueOriginType Origin { get; set; }

    public IExpression Expression { get; set; } = new ExpressionDto();

    public ITags Tags { get; set; } = new TagsDto();

    public string PrimitiveString { get; set; } = StringConstants.NaN;
}
