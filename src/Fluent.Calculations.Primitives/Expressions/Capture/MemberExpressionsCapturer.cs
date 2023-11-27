namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq;
using System.Linq.Expressions;

public class MemberExpressionsCapturer : ExpressionVisitor, IMemberExpressionsCapturer
{
    private Dictionary<string, MemberExpression> capturedMemberExpressions = new Dictionary<string, MemberExpression>();

    public MemberExpression[] Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lamdaExpression) where TExpressionResulValue : class, IValueProvider
    {
        capturedMemberExpressions = new Dictionary<string, MemberExpression>();

        Visit(lamdaExpression);

        return capturedMemberExpressions.Values.ToArray();
    }

    protected override Expression VisitMember(MemberExpression node) => base.VisitMember(CaptureIfNotExists(node));

    private MemberExpression CaptureIfNotExists(MemberExpression node)
    {
        if (!typeof(IValueProvider).IsAssignableFrom(node.Type))
            return node;

        string memberName = node.Member.Name;

        if (!capturedMemberExpressions.ContainsKey(memberName))
            capturedMemberExpressions.Add(memberName, node);

        return node;
    }
}