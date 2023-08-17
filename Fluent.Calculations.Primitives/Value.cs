namespace Fluent.Calculations.Primitives;

public abstract class Value : IValue, IName
{
    protected Value(string name, decimal primitiveValue)
    {
        Name = name;
        PrimitiveValue = primitiveValue;
        IsConstant = true;
    }

    public Value(string expressionName, ExpressionNode expressionNode, decimal primitiveValue, ArgumentsList arguments, TagsList tags)
    {
        Name = expressionName;
        Expresion = expressionNode;
        PrimitiveValue = primitiveValue;
        Arguments = arguments;
        Tags = tags;
        IsConstant = false;
    }

    protected Value(Value value)
    {
        Name = value.Name;
        Expresion = value.Expresion;
        PrimitiveValue = value.PrimitiveValue;
        IsConstant = value.IsConstant;
        Tags = new TagsList(value.Tags);
        Arguments = new ArgumentsList(value.Arguments);
    }

    protected Value(CreateValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Expresion = createValueArgs.Expresion;
        PrimitiveValue = createValueArgs.PrimitiveValue;
        IsConstant = createValueArgs.IsConstant;
        Tags = createValueArgs.Tags;
        Arguments = createValueArgs.Arguments;
    }

    public string Name { get; private set; }

    public ExpressionNode Expresion { get; }

    public decimal PrimitiveValue { get; private set; }

    public bool IsConstant { get; private set; }

    public ArgumentsList Arguments { get; } = ArgumentsList.Empty;

    public TagsList Tags { get; } = TagsList.Empty;

    public abstract IValue ToExpressionResult(CreateValueArgs args);

    void IName.Set(string name) => this.Name = name;

    public override string ToString() => $"{Name}: {PrimitiveValue:0.00}";
}
