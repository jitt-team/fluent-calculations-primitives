namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

internal interface IReflectionProvider
{
    bool IsParameter(MemberInfo member);

    bool IsEvaluation(MemberInfo member);

    IValue GetValue(Expression expression);

    string GetPropertyOrFieldName(MemberInfo expression);
}