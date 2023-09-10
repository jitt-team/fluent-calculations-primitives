namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class ExpressionMembersCapturer
{
    public ExpressionMembersCaptureResult Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue
    {
        List<object> capturedMembers = CaptureExpressionMembers(expression);

        return new ExpressionMembersCaptureResult(
            capturedMembers.Where(CapturedInputParameter.OfType).Cast<CapturedInputParameter>().ToArray(),
            capturedMembers.Where(CapturedEvaulation.OfType).Cast<CapturedEvaulation>().ToArray()
        );
    }

    private List<object> CaptureExpressionMembers(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Lambda)
            return CaptureExpressionMembers(((LambdaExpression)expression).Body);
        else if (expression.NodeType == ExpressionType.Convert)
            return CaptureExpressionMembers(((UnaryExpression)expression).Operand);
        else if (BinaryExpressionTypes.Contains(expression.NodeType))
            return CaptureBinaryExpression((BinaryExpression)expression);
        else if (expression.NodeType == ExpressionType.Conditional)
            return CaptureConditionalExpression((ConditionalExpression)expression);
        else if (expression.NodeType == ExpressionType.MemberAccess)
            return CaptureMemberExpression((MemberExpression)expression);
        else
            return new List<object>(); // TODO : Returm NotImplemented Value for visibility
    }

    private List<object> CaptureBinaryExpression(BinaryExpression expression) =>
        CaptureExpressionMembers(expression.Left).Concat(CaptureExpressionMembers(expression.Right)).ToList();

    private List<object> CaptureConditionalExpression(ConditionalExpression expression) =>
        CaptureExpressionMembers(expression.Test)
                .Concat(CaptureExpressionMembers(expression.IfTrue))
                .Concat(CaptureExpressionMembers(expression.IfFalse))
                .ToList();

    private List<object> CaptureMemberExpression(MemberExpression expression)
    {
        // TODO : Any way to make this once per member and not one each usage? Maybe invoke much later?
        if (IsInputParameter(expression.Member))
            return ToListResult(new CapturedInputParameter(DynamicInvoke(expression), expression.Member.Name));

        // Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree
        else if (IsEvaluation(expression.Member))
            return ToListResult(new CapturedEvaulation(((PropertyInfo)expression.Member).Name));

        return new List<object>();
    }

    private static List<object> ToListResult(object obj) => new List<object> { obj };

    private bool IsInputParameter(MemberInfo member) => member.MemberType == MemberTypes.Field
        || member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite;

    private bool IsEvaluation(MemberInfo member) => member.MemberType == MemberTypes.Property && !((PropertyInfo)member).CanWrite;

    private IValue DynamicInvoke(Expression expression) => (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new InvalidOperationException(@$"Expression ""{body}"" resulted in Null");

    private static ExpressionType[] BinaryExpressionTypes = new[] {
        ExpressionType.Add,
        ExpressionType.Subtract,
        ExpressionType.Multiply,
        ExpressionType.Divide,
        ExpressionType.GreaterThan,
        ExpressionType.LessThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual,
        ExpressionType.Modulo,
        ExpressionType.Power,
        ExpressionType.Equal,
        ExpressionType.NotEqual,
        ExpressionType.Add,
        ExpressionType.AndAlso,
        ExpressionType.OrElse,
        ExpressionType.Or,
    };
}