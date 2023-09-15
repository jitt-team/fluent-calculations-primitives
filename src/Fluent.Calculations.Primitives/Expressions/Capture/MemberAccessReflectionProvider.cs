﻿namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberAccessReflectionProvider : IMemberAccessReflectionProvider
{
    public bool IsInputMember(MemberInfo member) => member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite;

    public bool IsEvaluationMember(MemberInfo member) => member.MemberType == MemberTypes.Property && !((PropertyInfo)member).CanWrite;

    public IValue GetValue(Expression expression) => (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);

    private object EnsureNotNull(object? obj, Expression body) => obj ?? throw new NullExpressionResultException(body.ToString());

    public string GetPropertyName(MemberExpression expression) => ((PropertyInfo)expression.Member).Name;
}