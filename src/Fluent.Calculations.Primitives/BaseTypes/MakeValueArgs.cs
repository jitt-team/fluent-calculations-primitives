namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

/// <include file="Docs.xml" path='*/MakeValueArgs/class/*' />
public class MakeValueArgs
{
    public string Name { get; private set; } = StringConstants.NaN;

    public ExpressionNode Expression { get; private set; } = ExpressionNode.None;

    public decimal PrimitiveValue { get; private set; }

    public ValueOriginType Origin { get; private set; }

    public ArgumentsCollection Arguments { get; private set; } = ArgumentsCollection.Empty;

    public TagsCollection Tags { get; private set; } = TagsCollection.Empty;

    public string Scope { get; private set; } = StringConstants.NaN;

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue) =>
        Compose(name, expression, primitiveValue, ValueOriginType.Constant, StringConstants.NaN);

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue, ValueOriginType origin) =>
        Compose(name, expression, primitiveValue, origin, StringConstants.NaN);

    internal static MakeValueArgs Compose(string name, ExpressionNode expression, decimal primitiveValue, ValueOriginType origin, string scope) =>
        new()
        {
            Name = name,
            Expression = expression,
            PrimitiveValue = primitiveValue,
            Origin = origin,
            Scope = scope
        };

    public MakeValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsCollection(tags);
        return this;
    }
}
