namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

internal interface IMemberExpressionsCapturer
{
    MemberExpression[] Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lamdaExpression) where TExpressionResulValue : class, IValueProvider;
}