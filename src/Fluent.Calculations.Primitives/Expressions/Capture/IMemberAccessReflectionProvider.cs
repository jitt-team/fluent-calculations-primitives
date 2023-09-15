using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Calculations.Primitives.Expressions.Capture
{
    internal interface IMemberAccessReflectionProvider
    {
        bool IsInputMember(MemberInfo member);

        bool IsEvaluationMember(MemberInfo member);

        IValue GetValue(Expression expression);

        string GetPropertyName(MemberExpression expression);
    }
}