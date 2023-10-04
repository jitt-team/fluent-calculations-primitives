namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberExpressionValueCapturer : IMemberExpressionValueCapturer
{
    private readonly IMemberExpressionsCapturer memberExpressionsCapturer;
    private readonly IReflectionProvider reflectionProvider;
    private readonly IValuesCache parameterCache;

    public MemberExpressionValueCapturer() : this(new MemberExpressionsCapturer(), new ReflectionProvider(), new ValuesCache()) { }

    public MemberExpressionValueCapturer(IMemberExpressionsCapturer membersCapturer, IReflectionProvider reflectionProvider, IValuesCache valuesCache)
    {
        this.memberExpressionsCapturer = membersCapturer;
        this.reflectionProvider = reflectionProvider;
        this.parameterCache = valuesCache;
    }

    public CapturedExpressionMembers Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lambdaExpression) where TExpressionResulValue : class, IValue
    {
        var parameters = new List<CapturedParameterMember>();
        var evaluations = new List<CapturedEvaluationMember>();

        MemberExpression[] lambdaMemberExpressions = memberExpressionsCapturer.Capture(lambdaExpression);

        foreach (MemberExpression memberExpression in lambdaMemberExpressions)
            if (IsParameter(memberExpression.Member))
                parameters.Add(ToParameter(memberExpression));
            else if (IsEvaluation(memberExpression.Member))
                evaluations.Add(ToEvaluation(memberExpression));

        return new CapturedExpressionMembers(parameters, evaluations);
    }

    private bool IsParameter(MemberInfo memberInfo) => reflectionProvider.IsParameter(memberInfo);

    private bool IsEvaluation(MemberInfo memberInfo) => reflectionProvider.IsEvaluation(memberInfo);

    private CapturedParameterMember ToParameter(MemberExpression memberExpression)
    {
        string name = GetName(memberExpression.Member);

        return new CapturedParameterMember(GetValue(memberExpression, name), name);
    }

    private CapturedEvaluationMember ToEvaluation(MemberExpression memberExpression) => new CapturedEvaluationMember(GetName(memberExpression.Member));

    private IValue GetValue(MemberExpression expression, string name)
    {
        if (!name.Equals(Constants.NaN) && parameterCache.ContainsKey(name))
            return parameterCache.GetByKey(name);

        IValue value = reflectionProvider.GetValue(expression);

        if (!name.Equals(Constants.NaN))
            parameterCache.Add(name, value);

        return value;
    }

    private string GetName(MemberInfo memberInfo) => reflectionProvider.GetPropertyOrFieldName(memberInfo);
}


