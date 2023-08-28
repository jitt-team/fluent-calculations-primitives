namespace Fluent.Calculations.Primitives.BaseTypes;

using Fluent.Calculations.Primitives.Expressions;

public class CreateValueArgs
{
    public string Name { get; private set; } = "NotProvided";

    public ExpressionNode Expresion { get; private set; } = ExpressionNode.Default;

    public decimal PrimitiveValue { get; private set; }

    public bool IsConstant { get; private set; }

    public ArgumentsList Arguments { get; private set; } = ArgumentsList.Empty;

    public TagsList Tags { get; private set; } = TagsList.Empty;

    internal static CreateValueArgs Compose(string name, ExpressionNode expression, decimal value) =>
        new CreateValueArgs
        {
            Name = name,
            Expresion = expression,
            PrimitiveValue = value,
            IsConstant = false
        };

    public CreateValueArgs WithTags(params Tag[] tags)
    {
        Tags = new TagsList(tags);
        return this;
    }
}
