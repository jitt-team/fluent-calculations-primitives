using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives;

public abstract class Value : IValue, IName
{
    public override string ToString() => $"{Name}";

    private Value() { }

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
        Expression = createValueArgs.Expresion;
        Tags = createValueArgs.Tags;
    }

    public string Name { get; private set; } = string.Empty;

    public ExpressionNode Expression { get; protected set; } = ExpressionNode.Default;

    public decimal PrimitiveValue { get; private set; } = 0;

    public bool IsConstant { get; private set; } = true;

    public TagsList Tags { get; } = TagsList.Empty;

    public abstract IValue ToExpressionResult(CreateValueArgs args);

    void IName.Set(string name) => this.Name = name;

    public ResultType Return<ResultType, ResultPrimitiveType>(
            IValue right,
            Func<IValue, IValue, ResultPrimitiveType> calcFunc,
            string operatorName) where ResultType : IValue, new()
    {
        ExpressionNode operationNode = ExpressionNodeMath
            .Create(ComposeBinaryExpressionBody())
            .WithArguments(this, right);

        return (ResultType)new ResultType().ToExpressionResult(CreateValueArgs
            .Compose(operatorName, operationNode, Convert.ToDecimal(calcFunc(this, right))));

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
