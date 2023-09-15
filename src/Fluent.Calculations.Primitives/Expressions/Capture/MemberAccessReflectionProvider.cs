namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberAccessReflectionProvider : IMemberAccessReflectionProvider
{
    public bool IsInputMember(MemberInfo member) => member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite;

    public bool IsEvaluationMember(MemberInfo member) => member.MemberType == MemberTypes.Property && !((PropertyInfo)member).CanWrite;

    public IValue GetValue(Expression expression)
    {
        // TODO: A potential place to cache compiled member access expressions for performance
        return (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);
    }

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new NullExpressionResultException(body.ToString());

    public string GetPropertyName(MemberExpression expression)
    {
        MemberInfo memberInfo = expression.Member;

        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)expression.Member).Name;
            case MemberTypes.Property:
                return ((PropertyInfo)expression.Member).Name;
            default:
                break;
        }

        throw new NotSupportedException($"Member type {memberInfo.MemberType} of [{expression.Member.Name}] is not supported.");
    }
}
