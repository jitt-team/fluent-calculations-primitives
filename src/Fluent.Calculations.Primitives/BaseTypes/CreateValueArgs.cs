﻿namespace Fluent.Calculations.Primitives.BaseTypes;

using Fluent.Calculations.Primitives.Expressions;

public class CreateValueArgs
{
    public string Name { get; private set; } = "NaN";

    public ExpressionNode Expression { get; private set; } = ExpressionNode.None;

    public decimal PrimitiveValue { get; private set; }

    public bool IsConstant { get; private set; }

    public ArgumentsCollection Arguments { get; private set; } = ArgumentsCollection.Empty;

    public TagsCollection Tags { get; private set; } = TagsCollection.Empty;

    internal static CreateValueArgs Build(string name, ExpressionNode expression, decimal primitiveValue) =>
        new CreateValueArgs
        {
            Name = name,
            Expression = expression,
            PrimitiveValue = primitiveValue,
            IsConstant = false
        };

    public CreateValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsCollection(tags);
        return this;
    }
}
