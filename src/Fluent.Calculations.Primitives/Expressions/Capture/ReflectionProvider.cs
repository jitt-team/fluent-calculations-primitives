namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class ReflectionProvider : IReflectionProvider
{
    public bool IsParameter(MemberInfo member) => IsOfTypeIValue(member) && ParameterAndEvaluationDefaultConvention.IsFieldOrWritableProperty(member);

    public bool IsEvaluation(MemberInfo member) => IsOfTypeIValue(member) && ParameterAndEvaluationDefaultConvention.IsReadOnlyProperty(member);

    public IValueProvider GetValue(Expression expression)
    {
        // TODO: A potential place to cache compiled member access expressions for performance
        return (IValueProvider)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);
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

    private bool IsOfTypeIValue(MemberInfo member) => typeof(IValueProvider).IsAssignableFrom(GetFieldOrPropertyType(member));

    private Type GetFieldOrPropertyType(MemberInfo memberInfo)
    {
        switch (memberInfo.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)memberInfo).FieldType;
            case MemberTypes.Property:
                return ((PropertyInfo)memberInfo).PropertyType;
            default:
                break;
        }

        throw new NotSupportedException($"Member type {memberInfo.MemberType} of [{memberInfo.Name}] is not supported.");
    }

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new NullExpressionResultException(body.ToString());

    private static class ParameterAndEvaluationDefaultConvention
    {
        // TODO: Check type, should be IValue
        public static bool IsFieldOrWritableProperty(MemberInfo member) => 
            member.MemberType == MemberTypes.Field || (member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite);

        // TODO: Check type, should be IValue
        public static bool IsReadOnlyProperty(MemberInfo member) => 
            member.MemberType == MemberTypes.Property && !((PropertyInfo)member).CanWrite;
    }
}