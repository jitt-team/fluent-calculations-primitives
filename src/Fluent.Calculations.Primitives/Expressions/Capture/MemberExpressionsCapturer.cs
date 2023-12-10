namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq;
using System.Linq.Expressions;

public class MemberExpressionsCapturer : ExpressionVisitor, IMemberExpressionsCapturer
{
    private Dictionary<string, MemberExpression> capturedMemberExpressions = [];

    public MemberExpression[] Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lamdaExpression) where TExpressionResulValue : class, IValueProvider
    {
        capturedMemberExpressions = [];

        Visit(lamdaExpression);

        return [.. capturedMemberExpressions.Values];
    }

    protected override Expression VisitMember(MemberExpression node) => base.VisitMember(CaptureIfNotExists(node));

    private MemberExpression CaptureIfNotExists(MemberExpression node)
    {
        if (!typeof(IValueProvider).IsAssignableFrom(node.Type))
            return node;

        capturedMemberExpressions.TryAdd(node.Member.Name, node);
        
        return node;
    }
}