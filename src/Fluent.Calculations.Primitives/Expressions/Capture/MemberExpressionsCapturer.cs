namespace Fluent.Calculations.Primitives.Expressions.Capture;

using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq;
using System.Linq.Expressions;

public class MemberExpressionsCapturer : ExpressionVisitor, IMemberExpressionsCapturer
{
    private readonly List<MemberExpression> capturedMemberExpressions = new List<MemberExpression>();

    public List<MemberExpression> Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lamdaExpression) where TExpressionResulValue : class, IValue
    {
        Visit(lamdaExpression);

        return capturedMemberExpressions;
    }

    protected override Expression VisitMember(MemberExpression node) => base.VisitMember(CaptureIfNotExists(node));

    private MemberExpression CaptureIfNotExists(MemberExpression node)
    {
        if (!capturedMemberExpressions.Any(WithSameName))
            capturedMemberExpressions.Add(node);

        return node;

        bool WithSameName(MemberExpression memberExpression) => memberExpression.Member.Name.Equals(node.Member.Name))
    }
}