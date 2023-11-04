namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;

/// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/class/*' />
[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
public abstract class Value : IValue, IOrigin
{
    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/prop-name/*' />
    public string Name { get; private set; }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/prop-expression/*' />
    public ExpressionNode Expression { get; init; }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/prop-primitive/*' />
    public decimal Primitive { get; init; }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/prop-origin/*' />
    public ValueOriginType Origin { get; protected set; }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/prop-tags/*' />
    public TagsCollection Tags { get; init; }

    private Value()
    {
        Name = StringConstants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/ctor-value/*' />
    public Value(Value value)
    {
        Name = value.Name;
        Expression = value.Expression;
        Primitive = value.Primitive;
        Origin = value.Origin;
        Tags = value.Tags;
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/ctor-args/*' />
    protected Value(MakeValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        Origin = createValueArgs.Origin;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-make/*' />
    public abstract IValue MakeOfThisType(MakeValueArgs args);

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-getdefault/*' />
    public abstract IValue GetDefault();

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-handlebinaryexpression/*' />
    public ResultType HandleBinaryOperation<ResultType, ResultPrimitiveType>(
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> expressionFunc,
        string operatorName) where ResultType : IValue, new() =>
        BinaryOperatorHandler.Handle<ResultType, ResultPrimitiveType>(this, right, expressionFunc, operatorName, ExpressionNodeType.BinaryExpression);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    IValue IOrigin.AsResult()
    {
        Origin = ValueOriginType.Result;
        return this;
    }

    void IOrigin.MarkAsParameter(string name)
    {
        Name = name;
        Origin = ValueOriginType.Parameter;
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-equals-value/*' />
    public bool Equals(IValue? value) => value != null && Primitive.Equals(value.Primitive);

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-equals-object/*' />
    public override bool Equals(object? obj)
    {
        if (obj is not IValue value) return false;
        return Equals(value);
    }

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-gethashcode/*' />
    public override int GetHashCode() => Primitive.GetHashCode();

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-tostring/*' />
    public override string ToString() => $"{Name}";

    /// <include file="IntelliSense.xml" path='docs/members[@name="Value"]/method-valuetostring/*' />
    public virtual string ValueToString() => $"{Primitive:0.00}";
}
