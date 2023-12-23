namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="Value"]/class/*' />
[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
public abstract class Value : IValueProvider, IOrigin
{
    public string Name { get; private set; }

    public IExpression Expression { get; init; }

    public decimal Primitive { get; init; }

    public ValueOriginType Origin { get; protected set; }

    public ITags Tags { get; init; }

    public string Scope { get; private set; }

    private Value()
    {
        Name = StringConstants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
        Scope = StringConstants.NaN;
    }

    public Value(Value value)
    {
        Name = value.Name;
        Expression = value.Expression;
        Primitive = value.Primitive;
        Origin = value.Origin;
        Tags = value.Tags;
        Scope = value.Scope;
    }

    protected Value(MakeValueArgs makeValueArgs)
    {
        Name = makeValueArgs.Name;
        Primitive = makeValueArgs.PrimitiveValue;
        Origin = makeValueArgs.Origin;
        Expression = makeValueArgs.Expression;
        Tags = makeValueArgs.Tags;
        Scope = makeValueArgs.Scope;
    }

    public abstract IValueProvider MakeOfThisType(MakeValueArgs args);

    public abstract IValueProvider MakeDefault();

    protected ResultType HandleBinaryOperation<ResultType, ResultPrimitiveType>(
        IValueProvider right,
        Func<IValueProvider, IValueProvider, ResultPrimitiveType> expressionFunc,
        string operatorName) where ResultType : IValueProvider, new() =>
        BinaryOperatorHandler.Handle<ResultType, ResultPrimitiveType>(this, right, expressionFunc, operatorName, ExpressionNodeType.BinaryExpression);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull() && Origin != ValueOriginType.NaN;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Type => this.GetType().Name;

    IValueProvider IOrigin.AsResult()
    {
        Origin = ValueOriginType.Result;
        return this;
    }

    void IOrigin.MarkAsParameter(string name, string scope)
    {
        Name = name;

        if (Origin == ValueOriginType.NaN)
            Origin = ValueOriginType.Parameter;

        if (Scope.Equals(StringConstants.NaN))
            Scope = scope;
    }

    public void SetScope(string scope) => Scope = scope;

    public bool Equals(IValueProvider? value) => value != null && Primitive.Equals(value.Primitive);

    public override bool Equals(object? obj)
    {
        if (obj is not IValueProvider value) return false;
        return Equals(value);
    }

    public override int GetHashCode() => Primitive.GetHashCode();

    public override string ToString() => $"{Name}";

    IValue IValueProvider.Accept(ValueVisitor visitor) => ArgumentsVisitorInvoker.VisitArguments(this, visitor);

    public virtual string PrimitiveString => $"{Primitive:0.00}";
}
