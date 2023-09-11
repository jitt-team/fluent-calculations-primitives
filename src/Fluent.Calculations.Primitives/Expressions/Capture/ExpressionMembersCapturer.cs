namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal class ExpressionMembersCapturer : IExpressionMembersCapturer
{
    private readonly IExpressionMemberExtractor _expressionMemberExtractor;
    private readonly IMemberAccessExtractor _memberAccessCapturer;

    public ExpressionMembersCapturer(IExpressionMemberExtractor memberExtractor, IMemberAccessExtractor memberAccessCapturer)
    {
        _expressionMemberExtractor = memberExtractor;
        _memberAccessCapturer = memberAccessCapturer;
    }

    public List<object> Capture(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Lambda)
            return Capture(((LambdaExpression)expression).Body);
        else if (expression.NodeType == ExpressionType.Convert)
            return Capture(((UnaryExpression)expression).Operand);
        else if (_expressionMemberExtractor.IsBinaryExpression(expression.NodeType))
            return CaptureMultiple(_expressionMemberExtractor.ExtractBinaryExpressionMembers((BinaryExpression)expression));
        else if (expression.NodeType == ExpressionType.Conditional)
            return CaptureMultiple(_expressionMemberExtractor.ExtractConditionalExpressionMembers((ConditionalExpression)expression));
        else if (expression.NodeType == ExpressionType.MemberAccess)
            return _memberAccessCapturer.CaptureParameterMember((MemberExpression)expression);
        else
            return new List<object>(); // TODO : Returm NotImplemented Value for visibility
    }

    private List<object> CaptureMultiple(Expression[] expressions) => expressions.SelectMany(Capture).ToList();
 }
