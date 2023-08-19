using System.Runtime.CompilerServices;

namespace Fluent.Calculations.Primitives;

public abstract class Value : IValue, IName
{
    protected Value(string name, decimal primitiveValue)
    {
        Name = name;
        PrimitiveValue = primitiveValue;
        IsConstant = true;
    }

    public Value(string expressionName, ExpressionNode expressionNode, decimal primitiveValue, ArgumentsList arguments, TagsList tags)
    {
        Name = expressionName;
        Expresion = expressionNode;
        PrimitiveValue = primitiveValue;
        Tags = tags;
        IsConstant = false;
    }

    protected Value(Value value)
    {
        Name = value.Name;
        Expresion = value.Expresion;
        PrimitiveValue = value.PrimitiveValue;
        IsConstant = value.IsConstant;
        Tags = new TagsList(value.Tags);
    }

    protected Value(CreateValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Expresion = createValueArgs.Expresion;
        PrimitiveValue = createValueArgs.PrimitiveValue;
        IsConstant = createValueArgs.IsConstant;
        Tags = createValueArgs.Tags;
    }

    public string Name { get; private set; }

    public ExpressionNode Expresion { get; protected set; }

    public decimal PrimitiveValue { get; private set; }

    public bool IsConstant { get; private set; }

    public TagsList Tags { get; } = TagsList.Empty;

    public abstract IValue ToExpressionResult(CreateValueArgs args);

    void IName.Set(string name) => this.Name = name;

    public override string ToString() => $"{Name}: {PrimitiveValue:0.00}";

    public ResultType Return<ResultType, ResultPrimitiveType>(
    IValue right,
    string languageOperator,
    Func<IValue, IValue, ResultPrimitiveType> calcFunc,
    string operatorName)
    where ResultType : IValue, new()
    {
        ExpressionNode operationNode = ExpressionNodeMath
            .Create($"{this} {languageOperator} {right}").WithArguments(this, right);
        return (ResultType)new ResultType().ToExpressionResult(CreateValueArgs
            .Compose(operatorName, operationNode, Convert.ToDecimal(calcFunc(this, right))));
    }
}
