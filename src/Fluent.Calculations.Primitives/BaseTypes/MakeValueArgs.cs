namespace Fluent.Calculations.Primitives.BaseTypes;

using Fluent.Calculations.Primitives.Expressions;
using System;

public class MakeValueArgs
{
    public string Name { get; private set; } = StringConstants.NaN;

    public ExpressionNode Expression { get; private set; } = ExpressionNode.None;

    public decimal PrimitiveValue { get; private set; }

    public bool IsParameter { get; private set; }

    public ArgumentsCollection Arguments { get; private set; } = ArgumentsCollection.Empty;

    public TagsCollection Tags { get; private set; } = TagsCollection.Empty;

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue) =>
        new MakeValueArgs
        {
            Name = name,
            Expression = expression,
            PrimitiveValue = primitiveValue,
            IsParameter = false
        };

    internal static object Compose(string fieldName, ExpressionNode expressionNode, object primitiveValue)
    {
        throw new NotImplementedException();
    }

    public MakeValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsCollection(tags);
        return this;
    }
}
