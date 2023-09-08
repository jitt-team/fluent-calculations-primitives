namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public abstract class Value : IValue, IName, IValueOrigin
{
    public override string ToString() => $"{Name}";

    public virtual string ValueToString() => $"{Primitive:0.00}";

    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public decimal Primitive { get; init; }

    public bool IsInput { get; protected set; }

    public bool IsOutput { get; private set; }

    public TagsCollection Tags { get; init; }

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
        IsInput = value.IsInput;
        Tags = value.Tags;
    }

    protected Value(CreateValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        Primitive = createValueArgs.PrimitiveValue;
        IsInput = createValueArgs.IsConstant;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public abstract IValue Create(CreateValueArgs args);

    public abstract IValue Default { get; }

    void IName.Set(string name) => Name = name;

    IValue IValueOrigin.MarkAsEndResult()
    {
        IsOutput = true;
        return this;
    }

    IValue IValueOrigin.MarkAsInput()
    {
        IsInput = true;
        return this;
    }

    public ResultType HandleBinaryExpression<ResultType, ResultPrimitiveType>(
            IValue right,
            Func<IValue, IValue, ResultPrimitiveType> calcFunc,
            string operatorName) where ResultType : IValue, new()
    {
        ExpressionNode expressionNode = new  ExpressionNode(ComposeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression)
                .WithArguments(this, right);

        return (ResultType)new ResultType().Create(CreateValueArgs.Create(operatorName, expressionNode, Convert.ToDecimal(calcFunc(this, right))));

        string ComposeBinaryExpressionBody() => $"{this} {ToLanguageOperator(operatorName)} {right}";
    }

    public bool Equals(IValue? value) => value != null && Primitive.Equals(value.Primitive);

    public override bool Equals(object? obj)
    {
        if (obj is not IValue value) return false;
        return Equals(value);
    }

    public override int GetHashCode() => Primitive.GetHashCode();

    private string ToLanguageOperator(string operatorName)
    {
        switch (operatorName)
        {
            case "And": return "&";
            case "Or": return "|";
            case "IsEqual": return "==";
            case "NotEqual": return "!=";
            case "LessThan": return "<";
            case "GreaterThan": return ">";
            case "LessThanOrEqual": return "<=";
            case "GreaterThanOrEqual": return ">=";
            case "Add": return "+";
            case "Substract": return "-";
            case "Multiply": return "*";
            case "Divide": return "/";
            default: return "#unknown_operator#";
        }
    }
}
