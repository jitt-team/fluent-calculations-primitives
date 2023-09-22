namespace Fluent.Calculations.Primitives.Expressions.Capture;

using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq;
using System.Linq.Expressions;

public class MemberExpressionsCapturer : ExpressionVisitor, IMemberExpressionsCapturer
{
    private readonly List<MemberExpression> memberExpressions = new List<MemberExpression>();

    public List<MemberExpression> Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lamdaExpression) where TExpressionResulValue : class, IValue
    {
        Visit(lamdaExpression);
        return memberExpressions;
    }

    protected override Expression VisitMember(MemberExpression node) => base.VisitMember(CaptureIfNotExists(node));

    private MemberExpression CaptureIfNotExists(MemberExpression node)
    {
        if (!memberExpressions.Any(e => e.Member.Name.Equals(node.Member.Name)))
            memberExpressions.Add(node);
        return node;
    }
}