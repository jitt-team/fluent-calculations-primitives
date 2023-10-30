namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public abstract class Value : IValue, IOrigin
{
    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public decimal Primitive { get; init; }

    public bool IsParameter { get; protected set; }

    public bool IsOutput { get; private set; }

    public TagsCollection Tags { get; init; }

    private Value()
    {
        Name = StringConstants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
    }

    public Value(Value value)
    {
        Name = value.Name;
        Expression = value.Expression;
        Primitive = value.Primitive;
        IsParameter = value.IsParameter;
        Tags = value.Tags;
    }

    protected Value(MakeValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        IsParameter = createValueArgs.IsParameter;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public abstract IValue MakeOfThisType(MakeValueArgs args);

    public abstract IValue Default { get; }

    public ResultType HandleBinaryOperation<ResultType, ResultPrimitiveType>(
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> expressionFunc,
        string operatorName) where ResultType : IValue, new() =>
        BinaryOperatorHandler.Handle<ResultType, ResultPrimitiveType>(this, right, expressionFunc, operatorName, ExpressionNodeType.BinaryExpression);

    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    IValue IOrigin.AsResult()
    {
        IsOutput = true;
        return this;
    }

    void IOrigin.MarkAsParameter(string name)
    {
        Name = name;
        IsParameter = true;
    }

    public bool Equals(IValue? value) => value != null && Primitive.Equals(value.Primitive);

    public override bool Equals(object? obj)
    {
        if (obj is not IValue value) return false;
        return Equals(value);
    }

    public override int GetHashCode() => Primitive.GetHashCode();

    public override string ToString() => $"{Name}";

    public virtual string ValueToString() => $"{Primitive:0.00}";
}
