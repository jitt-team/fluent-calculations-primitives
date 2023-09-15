namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

internal class ExpressionCapturer : IExpressionCapturer
{
    private readonly IExpressionMembersCapturer membersCapturer;

    public ExpressionCapturer() : this(new ExpressionMembersCapturer()) { }

    public ExpressionCapturer(IExpressionMembersCapturer membersCapturer) => this.membersCapturer = membersCapturer;

    public ExpressionCaptureResult Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue
    {
        List<object> capturedMembers = membersCapturer.Capture(expression);

        return new ExpressionCaptureResult(
            capturedMembers.Where(CapturedInputMember.IsOfType).Cast<CapturedInputMember>().ToArray(),
            capturedMembers.Where(CapturedEvaulationMember.IsOfType).Cast<CapturedEvaulationMember>().ToArray()
        );
    }
}