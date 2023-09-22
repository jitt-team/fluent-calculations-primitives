namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberExpressionValueCapturer : IMemberExpressionValueCapturer
{
    private readonly IMemberExpressionsCapturer memberExpressionsCapturer;
    private readonly IReflectionProvider reflectionProvider;

    public MemberExpressionValueCapturer() : this(new MemberExpressionsCapturer(), new ReflectionProvider()) { }

    public MemberExpressionValueCapturer(IMemberExpressionsCapturer membersCapturer, IReflectionProvider reflectionProvider)
    {
        this.memberExpressionsCapturer = membersCapturer;
        this.reflectionProvider = reflectionProvider;
    }

    public MemberExpressionValues Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lambdaExpression) where TExpressionResulValue : class, IValue
    {
        var parameters = new List<CapturedParameter>();
        var evaluations = new List<CapturedEvaluation>();

        List<MemberExpression> lambdaMemberExpressions = memberExpressionsCapturer.Capture(lambdaExpression);

        foreach (MemberExpression memberExpression in lambdaMemberExpressions)
            if (IsParameter(memberExpression.Member))
                parameters.Add(ToParameter(memberExpression));
            else if (IsEvaluation(memberExpression.Member))
                evaluations.Add(ToEvaluation(memberExpression));

        // ..else
        // TODO: Consider collecting unknown members, perhaps some inline constants?
        // TODO: Handle Unknown members, throw exception early, explain why it shouldn't happen
        // TODO: Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree

        return new MemberExpressionValues(parameters, evaluations);
    }

    private bool IsParameter(MemberInfo memberInfo) => reflectionProvider.IsParameter(memberInfo);

    private bool IsEvaluation(MemberInfo memberInfo) => reflectionProvider.IsEvaluation(memberInfo);

    private CapturedParameter ToParameter(MemberExpression memberExpression) => new CapturedParameter(GetValue(memberExpression), GetName(memberExpression.Member));

    private CapturedEvaluation ToEvaluation(MemberExpression memberExpression) => new CapturedEvaluation(GetName(memberExpression.Member));

    private IValue GetValue(MemberExpression expression) => reflectionProvider.GetValue(expression);

    private string GetName(MemberInfo memberInfo) => reflectionProvider.GetPropertyOrFieldName(memberInfo);
}


