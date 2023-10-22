namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public class Values<T> : IValues<T>, IOrigin where T : class, IValue, new()
{
    public override string ToString() => $"{Name}";

    internal Values() : this(MakeValueArgs.Compose(StringConstants.Empty, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Collection), 0.00m)) { }

    private static Values<T> Empty = new Values<T>() ;

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

    public decimal Primitive { get; init; }

    public bool IsParameter { get; protected set; }

    public bool IsOutput { get; private set; }

    public TagsCollection Tags { get; init; }

    internal bool OriginIsSet => !string.IsNullOrEmpty(Name) && !Name.Equals(StringConstants.NaN);

    public IValue MakeOfThisType(MakeValueArgs args) => new Values<T>(args);

    public IValue MakeOfThisElementType(MakeValueArgs args) => new T().MakeOfThisType(args);

    public IValue Default => Empty;

    public bool IsSet => !string.IsNullOrEmpty(Name) && !Name.Equals(StringConstants.NaN);

    public int Count => Expression.Arguments.Count;

    public virtual string ValueToString() => $"{Primitive:0.00}";

    IEnumerator IEnumerable.GetEnumerator() => Expression.Arguments.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Expression.Arguments.Cast<T>().GetEnumerator();

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

    internal static Values<T> Of(Expression<Func<T[]>> valuesFunc, [CallerMemberName] string fieldName = "")
    {
        List<T> collectionElements = valuesFunc.Compile().Invoke().ToList();
        decimal primitiveValue = collectionElements.Sum(value => value.Primitive);
        var expressionNode = new ExpressionNode($"{typeof(T).Name}[{collectionElements.Count}]", ExpressionNodeType.Collection).WithArguments(collectionElements);
        Values<T> numbers = new Values<T>(MakeValueArgs.Compose(fieldName, expressionNode, primitiveValue));
        return numbers;
    }
}

