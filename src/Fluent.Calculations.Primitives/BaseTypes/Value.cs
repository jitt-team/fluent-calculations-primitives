namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;

[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
public abstract class Value : IValueProvider, IOrigin
{
    public string Name { get; private set; }

    public IExpression Expression { get; init; }

    public decimal Primitive { get; init; }

    public ValueOriginType Origin { get; protected set; }

    public ITags Tags { get; init; }

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
        Origin = value.Origin;
        Tags = value.Tags;
    }

    protected Value(MakeValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        Origin = createValueArgs.Origin;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public abstract IValueProvider MakeOfThisType(MakeValueArgs args);

    public abstract IValueProvider MakeDefault();

    public ResultType HandleBinaryOperation<ResultType, ResultPrimitiveType>(
        IValueProvider right,
        Func<IValueProvider, IValueProvider, ResultPrimitiveType> expressionFunc,
        string operatorName) where ResultType : IValueProvider, new() =>
        BinaryOperatorHandler.Handle<ResultType, ResultPrimitiveType>(this, right, expressionFunc, operatorName, ExpressionNodeType.BinaryExpression);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Type => this.GetType().Name;

    IValueProvider IOrigin.AsResult()
    {
        Origin = ValueOriginType.Result;
        return this;
    }

    void IOrigin.MarkAsParameter(string name)
    {
        Name = name;
        Origin = ValueOriginType.Parameter;
    }

    public bool Equals(IValueProvider? value) => value != null && Primitive.Equals(value.Primitive);

    public override bool Equals(object? obj)
    {
        if (obj is not IValueProvider value) return false;
        return Equals(value);
    }

    public override int GetHashCode() => Primitive.GetHashCode();

    public override string ToString() => $"{Name}";

    public virtual string PrimitiveString => $"{Primitive:0.00}";
}
