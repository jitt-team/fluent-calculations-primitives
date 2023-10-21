namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public interface IValues<ElementValueType> : IReadOnlyCollection<ElementValueType>, IValue where ElementValueType : class, IValue, new()
{
    IValue MakeElement(MakeValueArgs args);
}

public class Values<ElementValueType> : IValues<ElementValueType>, IOrigin where ElementValueType : class, IValue, new()
{
    public override string ToString() => $"{Name}";

    internal Values()
    {
        Name = Constants.NaN;
        Expression = ExpressionNode.None;
        Tags = TagsCollection.Empty;
    }

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

    internal bool OriginIsSet => !string.IsNullOrEmpty(Name) && !Name.Equals(Constants.NaN);

    public IValue Make(MakeValueArgs args) => throw new NotImplementedException();

    public IValue MakeElement(MakeValueArgs args) => new ElementValueType().Make(args);

    public IValue Default { get; }

    public bool IsSet => !string.IsNullOrEmpty(Name) && !Name.Equals(Constants.NaN);

    public int Count => Expression.Arguments.Count;

    public virtual string ValueToString() => $"{Primitive:0.00}";

    IEnumerator IEnumerable.GetEnumerator() => Expression.Arguments.GetEnumerator();

    IEnumerator<ElementValueType> IEnumerable<ElementValueType>.GetEnumerator() => Expression.Arguments.Cast<ElementValueType>().GetEnumerator();

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

    internal static Values<ElementValueType> Of(Expression<Func<Number[]>> valuesFunc, [CallerMemberName] string fieldName = "")
    {
        List<Number> collectionElements = valuesFunc.Compile().Invoke().ToList();
        decimal primitiveValue = collectionElements.Sum(value => value.Primitive);
        var expressionNode = new ExpressionNode($"{typeof(ElementValueType).Name}[{collectionElements.Count}]", ExpressionNodeType.Collection).WithArguments(collectionElements);
        Values<ElementValueType> numbers = new Values<ElementValueType>(MakeValueArgs.Compose(fieldName, expressionNode, primitiveValue));
        return numbers;
    }
}

