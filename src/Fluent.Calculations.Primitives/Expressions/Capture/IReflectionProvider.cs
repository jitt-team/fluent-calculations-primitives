using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluent.Calculations.Primitives.Expressions.Capture
{
    internal interface IReflectionProvider
    {
        bool IsParameter(MemberInfo member);

        bool IsEvaluation(MemberInfo member);

        IValue GetValue(Expression expression);

        string GetPropertyName(MemberExpression expression);
    }
}