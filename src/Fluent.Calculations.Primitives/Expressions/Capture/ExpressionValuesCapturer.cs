namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

internal class ExpressionValuesCapturer : IExpressionValuesCapturer
{
    private readonly IMemberExpressionsCapturer memberExpressionsCapturer;
    private readonly IReflectionProvider reflectionProvider;

    public ExpressionValuesCapturer() : this(
        new MemberExpressionsCapturer(),
        new ReflectionProvider())
    { }

    public ExpressionValuesCapturer(
        IMemberExpressionsCapturer membersCapturer,
        IReflectionProvider reflectionProvider)
    {
        this.memberExpressionsCapturer = membersCapturer;
        this.reflectionProvider = reflectionProvider;
    }

    public CapturedExpressionValues Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue
    {
        List<MemberExpression> memberExpressions = memberExpressionsCapturer.Capture(expression);

        CapturedParameter[] parameter = memberExpressions
            .Where(expression => reflectionProvider.IsParameter(expression.Member))
            .Select(expression => new CapturedParameter(
                reflectionProvider.GetValue(expression),
                reflectionProvider.GetPropertyName(expression)))
            .ToArray();

        CapturedEvaluation[] evaluations = memberExpressions
            .Where(expression => reflectionProvider.IsEvaluation(expression.Member))
            .Select(expression => new CapturedEvaluation(
                reflectionProvider.GetPropertyName(expression)))
            .ToArray();

        // TODO: Handle Unknown members, throw exception early, explain why it shouldn't happen
        // TODO: Any way to make this once per member and not one each usage? Maybe invoke much later?
        // TODO: Maybe we can capture just expressions to members and then invoke the at the end just once?
        // TODO: Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree

        return new CapturedExpressionValues(parameter, evaluations);
    }
}


