namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/Value/class/*'/>
[DebuggerDisplay("Name = {Name}, Value = {Primitive}")]
public abstract class Value : IValueProvider, IOrigin
{
    /// <include file="Docs.xml" path='*/Value/Name/*'/>
    public string Name { get; private set; }

    /// <include file="Docs.xml" path='*/Value/Expression/*'/>
    public IExpression Expression { get; init; }

    /// <include file="Docs.xml" path='*/Value/Primitive/*'/>
    public decimal Primitive { get; init; }

    /// <include file="Docs.xml" path='*/Value/PrimitiveString/*'/>
    public virtual string PrimitiveString => $"{Primitive:0.00}";

    /// <include file="Docs.xml" path='*/Value/Origin/*'/>
    public ValueOriginType Origin { get; protected set; }

    /// <include file="Docs.xml" path='*/Value/Tags/*'/>
    public ITags Tags { get; init; }

    /// <include file="Docs.xml" path='*/Value/Scope/*'/>
    public string Scope { get; private set; }

    /// <include file="Docs.xml" path='*/Value/ctor-value/*'/>
    public Value(Value value)
    {
        Name = value.Name;
        Expression = value.Expression;
        Primitive = value.Primitive;
        Origin = value.Origin;
        Tags = value.Tags;
        Scope = value.Scope;
    }

    /// <include file="Docs.xml" path='*/Value/ctor-makeValueArgs/*'/>
    protected Value(MakeValueArgs makeValueArgs)
    {
        Name = makeValueArgs.Name;
        Primitive = makeValueArgs.PrimitiveValue;
        Origin = makeValueArgs.Origin;
        Expression = makeValueArgs.Expression;
        Tags = makeValueArgs.Tags;
        Scope = makeValueArgs.Scope;
    }

    private Value()
    {
        Name = StringConstants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
        Scope = StringConstants.NaN;
    }

    /// <include file="Docs.xml" path='*/Value/MakeOfThisType/*'/>
    public abstract IValueProvider MakeOfThisType(MakeValueArgs args);

    /// <include file="Docs.xml" path='*/Value/MakeDefault/*'/>
    public abstract IValueProvider MakeDefault();

    /// <include file="Docs.xml" path='*/Value/HandleBinaryOperation/*'/>
    protected ResultType HandleBinaryOperation<ResultType, ResultPrimitiveType>(
        IValueProvider right,
        Func<IValueProvider, IValueProvider, ResultPrimitiveType> expressionFunc,
        string operatorName) where ResultType : IValueProvider, new() =>
        BinaryOperatorHandler.Handle<ResultType, ResultPrimitiveType>(this, right, expressionFunc, operatorName, ExpressionNodeType.BinaryExpression);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull() && Origin != ValueOriginType.NaN;

    /// <include file="Docs.xml" path='*/Value/Type/*'/>
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

    /// <include file="Docs.xml" path='*/Value/Equals-value/*'/>
    public bool Equals(IValueProvider? value) => value != null && Primitive.Equals(value.Primitive);

    /// <include file="Docs.xml" path='*/Value/Equals-obj/*'/>
    public override bool Equals(object? obj)
    {
        if (obj is not IValueProvider value) return false;
        return Equals(value);
    }

    /// <include file="Docs.xml" path='*/Value/GetHashCode/*'/>
    public override int GetHashCode() => Primitive.GetHashCode();

    /// <include file="Docs.xml" path='*/Value/ToString/*'/>
    public override string ToString() => $"{Name}";

    IValue IValueProvider.Accept(ValueVisitor visitor) => ArgumentsVisitorInvoker.VisitArguments(this, visitor);
}
