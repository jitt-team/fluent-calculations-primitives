namespace Fluent.Calculations.Primitives.BaseTypes;

using Fluent.Calculations.Primitives.Expressions;
using System;

public class MakeValueArgs
{
    public string Name { get; private set; } = StringConstants.NaN;

    public ExpressionNode Expression { get; private set; } = ExpressionNode.None;

    public decimal PrimitiveValue { get; private set; }

    public ValueOriginType Origin { get; private set; }

    public ArgumentsCollection Arguments { get; private set; } = ArgumentsCollection.Empty;

    public TagsCollection Tags { get; private set; } = TagsCollection.Empty;

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue) =>
        Compose(name, expression, primitiveValue, ValueOriginType.Constant);

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue, ValueOriginType origin) =>
        new()
        {
            Name = name,
            Expression = expression,
            PrimitiveValue = primitiveValue,
            Origin = origin
        };

    public MakeValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsCollection(tags);
        return this;
    }
}
