namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;
internal class ExpressionMembersCapturer
{
    public ExpressionMembersCaptureResult Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue
    {
        List<object> capturedMembers = CaptureExpressionMembers(expression);

        return new ExpressionMembersCaptureResult(
            capturedMembers.Where(InputParameterCapture.OfType).Cast<InputParameterCapture>().ToArray(),
            capturedMembers.Where(PointerToEvaulationCapture.OfType).Cast<PointerToEvaulationCapture>().ToArray()
        );
    }

    private List<object> CaptureExpressionMembers(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Lambda)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            return CaptureExpressionMembers(lambdaExpression.Body);
        }
        else if (BinaryExpressionTypes.Contains(expression.NodeType))
        {
            BinaryExpression binaryExpression = (BinaryExpression)expression;
            return CaptureExpressionMembers(binaryExpression.Left).Concat(CaptureExpressionMembers(binaryExpression.Right)).ToList();
        }
        else if (expression.NodeType == ExpressionType.Convert)
        {
            UnaryExpression unaryExpression = (UnaryExpression)expression;
            return CaptureExpressionMembers(unaryExpression.Operand);
        }
        else if (expression.NodeType == ExpressionType.Conditional)
        {
            ConditionalExpression conditionalExpression = (ConditionalExpression)expression;
            return CaptureExpressionMembers(conditionalExpression.Test)
                .Concat(CaptureExpressionMembers(conditionalExpression.IfTrue))
                .Concat(CaptureExpressionMembers(conditionalExpression.IfFalse))
                .ToList();
        }
        else if (expression.NodeType == ExpressionType.MemberAccess)
        {
            MemberExpression memberExpression = (MemberExpression)expression;

            if (IsInputParameter(memberExpression.Member))
                // TODO : Any way to make this once per member and not one each usage? Maybe invoke much later?
                return new List<object> { new InputParameterCapture((IValue)DynamicInvoke(expression), memberExpression.Member.Name) };

            else if (IsEvaluation(memberExpression.Member))
                // Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree
                return new List<object> { new PointerToEvaulationCapture(((PropertyInfo)memberExpression.Member).Name) };

            else
                return new List<object>();
        }
        // TODO : What else could happen?
        else
            return new List<object>();
    }

    private bool IsInputParameter(MemberInfo member) => member.MemberType == MemberTypes.Field
        || (member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite);

    private bool IsEvaluation(MemberInfo member) => member.MemberType == MemberTypes.Property
        && !((PropertyInfo)member).CanWrite;

    private object DynamicInvoke(Expression expression) => EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new InvalidOperationException(@$"Expression ""{body}"" resulted in Null");

    private static ExpressionType[] BinaryExpressionTypes = new[] {
        ExpressionType.Add,
        ExpressionType.Subtract,
        ExpressionType.Multiply,
        ExpressionType.Divide,
        ExpressionType.GreaterThan,
        ExpressionType.LessThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual
    };
}