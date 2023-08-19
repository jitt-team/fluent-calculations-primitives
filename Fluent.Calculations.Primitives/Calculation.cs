using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public abstract class Calculation<TResult> : IValue where TResult : class, IValue, new()
{
    private readonly ExpressionTranslator expressionPartTranslator = new ExpressionTranslator();

    public Calculation()
    { }

    Dictionary<string, IValue> valueAmountResults = new Dictionary<string, IValue>();

    public List<string> Tags => new List<string>();

    public decimal PrimitiveValue => Is(() => Calculate(), Expresion.GetType().Name).PrimitiveValue;

    public ExpressionNode Expresion => Is(() => Calculate(), Expresion.GetType().Name).Expresion;

    public string Name { get; set; } = "TODO";

    public bool IsConstant => false;

    TagsList IValue.Tags => Return().Tags;

    public TResult Calculate()
    {
        SyncPublicFieldNames();
        return Return();
    }

    private void SyncPublicFieldNames()
    {
        var publicFields = this.GetType()
            .GetFields()
            .Where(fieldInfo => fieldInfo.IsPublic
            && typeof(IName).IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => new
            {
                Value = fieldInfo.GetValue(this) as IName,
                FieldName = fieldInfo.Name
            }).ToList();

        publicFields.ForEach(field => field?.Value?.Set(field.FieldName));
    }

    public abstract TResult Return();

    public IValue ToExpressionResult(CreateValueArgs args) => new TResult().ToExpressionResult(args);

    protected ExpressionResultValue Is<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> expression,
        [CallerMemberName] string name = "",
        [CallerArgumentExpression("expression")] string lambdaExpressionBody = "") where ExpressionResultValue : class, IValue
    {
        string prefixedName = $"ƒ:{name}";
        if (valueAmountResults.TryGetValue(prefixedName, out IValue cachedValue))
            return (ExpressionResultValue)cachedValue;

        ExpressionNode expressionNode = expressionPartTranslator.Translate(expression, lambdaExpressionBody);
        ExpressionResultValue result = expression.Compile().Invoke();

        if (!expressionNode.Arguments.Any())
            expressionNode.Arguments.AddRange(result.Expresion.Arguments);

        IValue value = result.ToExpressionResult(CreateValueArgs.Compose(prefixedName, expressionNode, result.PrimitiveValue));

        valueAmountResults.Add(prefixedName, value);

        return (ExpressionResultValue)value;
    }


}