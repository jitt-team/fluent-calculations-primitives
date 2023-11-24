namespace Fluent.Calculations.Primitives.Collections;

using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ValuesDebugView))]
public class Values<T> : IValues<T>, IOrigin where T : class, IValue, new()
{
    public override string ToString() => $"{Name}";

    internal Values() : this(MakeValueArgs.Compose(StringConstants.Empty, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Collection), 0.00m)) { }

    private static readonly Values<T> Empty = new();

    public void Add(T value, [CallerMemberName] string fieldName = "")
    {
        Expression.Arguments.Add(value);
        Expression.UpdateBody(ComposeExpressionBody(Expression.Arguments.Count));
        Primitive += value.Primitive;
        Name = fieldName;
        Origin = value.Origin;
    }

    protected Values(MakeValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        Origin = createValueArgs.Origin;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public decimal Primitive { get; private set; }

    public ValueOriginType Origin { get; protected set; }

    public TagsCollection Tags { get; init; }

    public IValue MakeOfThisType(MakeValueArgs args) => new Values<T>(args);

    public IValue MakeOfThisElementType(MakeValueArgs args) => new T().MakeOfThisType(args);

    public IValue GetDefault() => Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Type => GetType().Name;

    public int Count => Expression.Arguments.Count;

    public virtual string PrimitiveString => $"{Primitive:0.00}";

    IEnumerator IEnumerable.GetEnumerator() => Expression.Arguments.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Expression.Arguments.Cast<T>().GetEnumerator();

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

    internal static Values<T> SumOf(Expression<Func<T[]>> valuesFunc, [CallerMemberName] string fieldName = "")
    {
        List<T> collectionElements = valuesFunc.Compile().Invoke().ToList();
        decimal primitiveValue = collectionElements.Sum(value => value.Primitive);
        var expressionNode = new ExpressionNode(ComposeExpressionBody(collectionElements.Count), ExpressionNodeType.Collection).WithArguments(collectionElements);
        Values<T> numbers = new(MakeValueArgs.Compose(fieldName, expressionNode, primitiveValue));
        return numbers;
    }

    private static string ComposeExpressionBody(int elementCount) => $"{typeof(T).Name}[{elementCount}]";

    public string ToJson()
    {
        throw new NotImplementedException("Not yet supported");
    }
}

public class ValuesDebugView
{
    private readonly IValueMetadata collectionValue;

    public ValuesDebugView(IValueMetadata collectionValue) => this.collectionValue = collectionValue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValueMetadata[] Items => collectionValue.Expression.Arguments.ToArray();
}