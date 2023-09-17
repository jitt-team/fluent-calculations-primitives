namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class ReflectionProvider : IReflectionProvider
{
    // TODO: Check type, should be IValue
    public bool IsParameter(MemberInfo member) => member.MemberType == MemberTypes.Field
        || (member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite);

    // TODO: Check type, should be IValue
    public bool IsEvaluation(MemberInfo member) => member.MemberType == MemberTypes.Property
        && !((PropertyInfo)member).CanWrite;

    public IValue GetValue(Expression expression)
    {
        // TODO: A potential place to cache compiled member access expressions for performance
        return (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);
    }

    public string GetPropertyOrFieldName(MemberInfo memberInfo)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)memberInfo).Name;
            case MemberTypes.Property:
                return ((PropertyInfo)memberInfo).Name;
            default:
                break;
        }

        throw new NotSupportedException($"Member type {memberInfo.MemberType} of [{memberInfo.Name}] is not supported.");
    }

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new NullExpressionResultException(body.ToString());
}
