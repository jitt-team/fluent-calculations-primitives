namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

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
        List<CapturedParameter> parameters = new List<CapturedParameter>();
        List<CapturedEvaluation> evaluations = new List<CapturedEvaluation>();

        foreach (MemberExpression memberExpression in memberExpressions)
        {
            if (IsParameter(memberExpression.Member))
                parameters.Add(ToParameter(memberExpression));
            else if (IsEvaluation(memberExpression.Member))
                evaluations.Add(ToEvaluation(memberExpression));

            // ..else
            // TODO: Consider collecting unknown members, perhaps some inline constants?
        }

        // TODO: Handle Unknown members, throw exception early, explain why it shouldn't happen
        // TODO: Any way to make this once per member and not one each usage? Maybe invoke much later?
        // TODO: Maybe we can capture just expressions to members and then invoke the at the end just once?
        // TODO: Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree

        return new CapturedExpressionValues(parameters.ToArray(), evaluations.ToArray());
    }


    private bool IsParameter(MemberInfo memberInfo) => reflectionProvider.IsParameter(memberInfo);

    private bool IsEvaluation(MemberInfo memberInfo) => reflectionProvider.IsEvaluation(memberInfo);

    private CapturedParameter ToParameter(MemberExpression memberExpression) => new CapturedParameter(
        GetValue(memberExpression),
        GetName(memberExpression.Member));

    private CapturedEvaluation ToEvaluation(MemberExpression memberExpression) => new CapturedEvaluation(
        GetName(memberExpression.Member));

    private IValue GetValue(MemberExpression expression) => reflectionProvider.GetValue(expression);

    private string GetName(MemberInfo memberInfo) => reflectionProvider.GetPropertyOrFieldName(memberInfo);
}


