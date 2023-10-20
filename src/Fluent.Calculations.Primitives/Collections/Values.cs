namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;

public abstract class Values<T> : IEnumerable<IValue>, IValue
{
    private readonly List<IValue> values = new List<IValue>();

    private Values()
    {
        Name = Constants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
    }

    protected Values(MakeValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        IsParameter = createValueArgs.IsParameter;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public abstract decimal Primitive { get; init; }

    public bool IsParameter { get; protected set; }

    public bool IsOutput { get; private set; }

    public TagsCollection Tags { get; init; }

    internal bool OriginIsSet => !string.IsNullOrEmpty(Name) && !Name.Equals(Constants.NaN);

    public void Add(IValue number) => values.Add(number);

    public IEnumerator<IValue> GetEnumerator() => values.GetEnumerator();

    public abstract IValue Make(MakeValueArgs args);

    public abstract IValue Default { get; }

    public virtual string ValueToString() => $"{Primitive:0.00}";

    IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();
}

