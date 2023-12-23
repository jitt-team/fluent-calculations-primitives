namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="Values"]/class/*' />
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ValuesDebugView))]
public class Values<T> : IValuesProvider<T>, IOrigin where T : class, IValueProvider, new()
{
    private readonly ExpressionNode expression;
    private readonly TagsCollection tags;

    public override string ToString() => $"{Name}";

    internal Values() : this(MakeValueArgs.Compose(StringConstants.Empty, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Collection), 0.00m)) { }

    private static readonly Values<T> Empty = [];

    public void Add(T value, [CallerMemberName] string fieldName = StringConstants.NaN)
    {
        expression.AppendArgument(value);
        expression.SetBody(ComposeExpressionBody(Expression.Arguments.Count));

        Primitive += value.Primitive;
        Name = fieldName;
        Origin = value.Origin;
        Scope = value.Scope;
    }

    protected Values(MakeValueArgs makeValueArgs)
    {
        expression = makeValueArgs.Expression;
        tags = makeValueArgs.Tags;
        Name = makeValueArgs.Name;
        Primitive = makeValueArgs.PrimitiveValue;
        Origin = makeValueArgs.Origin;
        Scope = makeValueArgs.Scope;
    }

    public string Name { get; private set; }

    public decimal Primitive { get; private set; }

    public ValueOriginType Origin { get; protected set; }

    public IExpression Expression => expression;

    public ITags Tags => tags;

    public string Scope { get; private set; }

    public IValueProvider MakeOfThisType(MakeValueArgs args) => new Values<T>(args);

    public IValueProvider MakeOfThisElementType(MakeValueArgs args) => new T().MakeOfThisType(args);

    public IValueProvider MakeDefault() => Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Type => GetType().Name;

    public int Count => Expression.Arguments.Count;

    public virtual string PrimitiveString => $"{Primitive:0.00}";

    IEnumerator IEnumerable.GetEnumerator() => Expression.Arguments.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Expression.Arguments.Cast<T>().GetEnumerator();

    IValueProvider IOrigin.AsResult()
    {
        Origin = ValueOriginType.Result;
        return this;
    }

    void IOrigin.MarkAsParameter(string name, string scope)
    {
        Name = name;
        Origin = ValueOriginType.Parameter;
    }

    internal static Values<T> ListOf(Expression<Func<T[]>> valuesFunc, [CallerMemberName] string fieldName = StringConstants.NaN)
    {
        List<T> collectionElements = [.. valuesFunc.Compile().Invoke()];
        decimal primitiveValue = collectionElements.Sum(value => value.Primitive);
        var expressionNode = new ExpressionNode(ComposeExpressionBody(collectionElements.Count), ExpressionNodeType.Collection).WithArguments(collectionElements);
        Values<T> numbers = new(MakeValueArgs.Compose(fieldName, expressionNode, primitiveValue));
        return numbers;
    }

    private static string ComposeExpressionBody(int elementCount) => $"{typeof(T).Name}[{elementCount}]";

    public IValue Accept(ValueVisitor visitor) => ArgumentsVisitorInvoker.VisitArguments(this, visitor);
}