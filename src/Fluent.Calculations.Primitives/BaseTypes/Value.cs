namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public abstract class Value : IValue, IName
{
    public override string ToString() => $"{Name}";

    public string Name { get; private set; }

    public ExpressionNode Expression { get; init; }

    public decimal PrimitiveValue { get; init; }

    public bool IsConstant { get; init; }

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
        PrimitiveValue = value.PrimitiveValue;
        IsConstant = value.IsConstant;
        Tags = value.Tags;
    }

    protected Value(CreateValueArgs createValueArgs)
    {
        Name = createValueArgs.Name;
        PrimitiveValue = createValueArgs.PrimitiveValue;
        IsConstant = createValueArgs.IsConstant;
        Expression = createValueArgs.Expression;
        Tags = createValueArgs.Tags;
    }

    public abstract IValue Create(CreateValueArgs args);

    public abstract IValue Default { get; }

    void IName.Set(string name) => Name = name;

    public ResultType HandleBinaryExpression<ResultType, ResultPrimitiveType>(
            IValue right,
            Func<IValue, IValue, ResultPrimitiveType> calcFunc,
            string operatorName) where ResultType : IValue, new()
    {
        ExpressionNode operationNode = ExpressionNodeBinaryExpression
            .Create(ComposeBinaryExpressionBody())
            .WithArguments(this, right);

        return (ResultType)new ResultType().Create(CreateValueArgs
            .Create(operatorName, operationNode, Convert.ToDecimal(calcFunc(this, right))));

        string ComposeBinaryExpressionBody() => $"{this} {ToLanguageOperator(operatorName)} {right}";
    }

    public bool Eqauls(IValue? value) => value == null ? false : PrimitiveValue.Equals(value.PrimitiveValue);

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => PrimitiveValue.GetHashCode();

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
