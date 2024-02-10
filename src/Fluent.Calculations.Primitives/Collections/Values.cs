namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/Values/class/*'/>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ValuesDebugView))]
public class Values<T> : IValuesProvider<T>, IOrigin where T : class, IValueProvider, new()
{
    private readonly ExpressionNode expression;
    private readonly TagsCollection tags;

    /// <include file="Docs.xml" path='*/Values/ToString/*'/>
    public override string ToString() => $"{Name}";

    internal Values() : this(MakeValueArgs.Compose(StringConstants.Empty, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Collection), 0.00m)) { }

    private static readonly Values<T> Empty = [];

    /// <include file="Docs.xml" path='*/Values/Add/*'/>
    public void Add(T value)
    {
        expression.AppendArgument(value);
        expression.SetBody(ComposeExpressionBody(Expression.Arguments.Count));
        Primitive += value.Primitive;
    }

    /// <include file="Docs.xml" path='*/Values/ctor-makeValueArgs/*'/>
    protected Values(MakeValueArgs makeValueArgs)
    {
        expression = makeValueArgs.Expression;
        tags = makeValueArgs.Tags;
        Name = makeValueArgs.Name;
        Primitive = makeValueArgs.PrimitiveValue;
        Origin = makeValueArgs.Origin;
        Scope = makeValueArgs.Scope;
    }

    /// <include file="Docs.xml" path='*/Values/Name/*'/>
    public string Name { get; private set; }

    /// <include file="Docs.xml" path='*/Values/Primitive/*'/>
    public decimal Primitive { get; private set; }

    /// <include file="Docs.xml" path='*/Values/Origin/*'/>
    public ValueOriginType Origin { get; protected set; }

    /// <include file="Docs.xml" path='*/Values/Expression/*'/>
    public IExpression Expression => expression;

    /// <include file="Docs.xml" path='*/Values/Tags/*'/>
    public ITags Tags => tags;

    /// <include file="Docs.xml" path='*/Values/Scope/*'/>
    public string Scope { get; private set; }

    /// <include file="Docs.xml" path='*/Values/MakeOfThisType/*'/>
    public IValueProvider MakeOfThisType(MakeValueArgs args) => new Values<T>(args);

    /// <include file="Docs.xml" path='*/Values/MakeOfThisElementType/*'/>
    public IValueProvider MakeOfThisElementType(MakeValueArgs args) => new T().MakeOfThisType(args);

    /// <include file="Docs.xml" path='*/Values/MakeDefault/*'/>
    public IValueProvider MakeDefault() => Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool IOrigin.IsSet => !Name.IsNaNOrNull();

    /// <include file="Docs.xml" path='*/Values/Type/*'/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Type => GetType().Name;

    /// <include file="Docs.xml" path='*/Values/Count/*'/>
    public int Count => Expression.Arguments.Count;

    /// <include file="Docs.xml" path='*/Values/PrimitiveString/*'/>
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
 
    /// <include file="Docs.xml" path='*/Values/Accept/*'/>
    public IValue Accept(ValueVisitor visitor) => ArgumentsVisitorInvoker.VisitArguments(this, visitor);

    internal static Values<T> ListOf(Expression<Func<T[]>> valuesFunc, [CallerMemberName] string fieldName = StringConstants.NaN)
    {
        List<T> collectionElements = [.. valuesFunc.Compile().Invoke()];
        decimal primitiveValue = collectionElements.Sum(value => value.Primitive);
        var expressionNode = new ExpressionNode(ComposeExpressionBody(collectionElements.Count), ExpressionNodeType.Collection).WithArguments(collectionElements);
        Values<T> numbers = new(MakeValueArgs.Compose(fieldName, expressionNode, primitiveValue));
        return numbers;
    }

    private static string ComposeExpressionBody(int elementCount) => $"{typeof(T).Name}[{elementCount}]";
}