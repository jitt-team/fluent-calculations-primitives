namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

/// <include file="Docs.xml" path='*/MakeValueArgs/class/*'/>
public class MakeValueArgs
{
    private MakeValueArgs() { }

    /// <include file="Docs.xml" path='*/MakeValueArgs/Name/*'/>
    public string Name { get; private set; } = StringConstants.NaN;

    /// <include file="Docs.xml" path='*/MakeValueArgs/Expression/*'/>
    public ExpressionNode Expression { get; private set; } = ExpressionNode.None;

    /// <include file="Docs.xml" path='*/MakeValueArgs/PrimitiveValue/*'/>
    public decimal PrimitiveValue { get; private set; }

    /// <include file="Docs.xml" path='*/MakeValueArgs/Origin/*'/>
    public ValueOriginType Origin { get; private set; }

    /// <include file="Docs.xml" path='*/MakeValueArgs/Arguments/*'/>
    public ArgumentsCollection Arguments { get; private set; } = ArgumentsCollection.Empty;

    /// <include file="Docs.xml" path='*/MakeValueArgs/Tags/*'/>
    public TagsCollection Tags { get; private set; } = TagsCollection.Empty;

    /// <include file="Docs.xml" path='*/MakeValueArgs/Scope/*'/>
    public string Scope { get; private set; } = StringConstants.NaN;

    /// <include file="Docs.xml" path='*/MakeValueArgs/WithTags/*'/>
    public MakeValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsCollection(tags);
        return this;
    }

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
}
