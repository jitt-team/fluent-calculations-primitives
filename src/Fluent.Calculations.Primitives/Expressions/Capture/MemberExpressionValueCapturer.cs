namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberExpressionValueCapturer : IMemberExpressionValueCapturer
{
    private readonly IMemberExpressionsCapturer memberExpressionsCapturer;
    private readonly IReflectionProvider reflectionProvider;
    private readonly IValuesCache parametersCache;
    private const string NaN = "NaN";

    public MemberExpressionValueCapturer() : this(new MemberExpressionsCapturer(), new ReflectionProvider(), new ValuesCache()) { }

    public MemberExpressionValueCapturer(IMemberExpressionsCapturer membersCapturer, IReflectionProvider reflectionProvider, IValuesCache valuesCache)
    {
        this.memberExpressionsCapturer = membersCapturer;
        this.reflectionProvider = reflectionProvider;
        this.parametersCache = valuesCache;
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

    private CapturedParameter ToParameter(MemberExpression memberExpression)
    {
        string name = GetName(memberExpression.Member);

        return new CapturedParameter(GetValue(memberExpression, name), name);
    }

    private CapturedEvaluation ToEvaluation(MemberExpression memberExpression) => new CapturedEvaluation(GetName(memberExpression.Member));

    private IValue GetValue(MemberExpression expression, string name)
    {
        if (!name.Equals(NaN) && parametersCache.ContainsKey(name))
            return parametersCache.GetByKey(name);

        IValue value = reflectionProvider.GetValue(expression);

        if (!name.Equals(NaN))
            parametersCache.Add(name, value);

        return value;
    }

    private string GetName(MemberInfo memberInfo) => reflectionProvider.GetPropertyOrFieldName(memberInfo);
}


