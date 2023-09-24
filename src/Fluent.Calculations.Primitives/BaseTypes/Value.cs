﻿namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Xml.Linq;

public abstract class Value : IValue, IName, IOrigin
{
    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public decimal Primitive { get; init; }

    public bool IsParameter { get; protected set; }

    public bool IsOutput { get; private set; }

    public TagsCollection Tags { get; init; }

    private bool originIsSet;

    private Value()
    {
        Name = "NaN";
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
        IsParameter = createValueArgs.IsConstant;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public abstract IValue Make(MakeValueArgs args);

    public abstract IValue Default { get; }

    public ResultType HandleBinaryExpression<ResultType, ResultPrimitiveType>(
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> calcFunc,
        string operatorName) where ResultType : IValue, new() =>
        BinaryExpressionHandler.Handle<ResultType, ResultPrimitiveType>(this, right, calcFunc, operatorName);

    void IName.Set(string name) => Name = name;

    bool IOrigin.IsSet => originIsSet;

    IValue IOrigin.AsResult()
    {
        if (!originIsSet)
        {
            IsOutput = true;
            originIsSet = true;
        }
        return this;
    }

    void IOrigin.MarkAsParameter(string name)
    {
        if (originIsSet)
            return;

        Name = name;
        IsParameter = true;
        originIsSet = true;
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
