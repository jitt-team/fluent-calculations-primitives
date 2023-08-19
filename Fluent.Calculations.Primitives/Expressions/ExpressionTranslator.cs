using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

internal class ExpressionTranslator
{
    public ExpressionNode Translate<ExpressionResultValue>(Expression<Func<ExpressionResultValue>> expression, [CallerArgumentExpression("expression")] string lambdaExpressionBody = "") where ExpressionResultValue : class, IValue
    {
        return TryTranslate(expression, AdjustLambdaPrefix(lambdaExpressionBody))
            .WithBody(AdjustLambdaPrefix(lambdaExpressionBody)); // TODO: incorporate into Node
    }

    private ExpressionNode TryTranslate<ExpressionResulType>(Expression<Func<ExpressionResulType>> expression, string lambdaExpressionBody) where ExpressionResulType : class, IValue
    {
        // TODO : Transalte various expressions to easy to read structure
        // TODO : Extend support of more syntaxes here
        if (expression.Body.NodeType == ExpressionType.Conditional)
        {
            ConditionalExpression conditionalExpression = (ConditionalExpression)expression.Body;

            IValue ifTrueValue = GetExpressionValue<IValue>(conditionalExpression.IfTrue);
            IValue ifFalseValue = GetExpressionValue<IValue>(conditionalExpression.IfFalse);
            Condition condition = GetExpressionValue<Condition>(conditionalExpression.Test);

            return new ExpressionNodeConditional(lambdaExpressionBody)
            {
                IfTrue = ifTrueValue,
                IfFalse = ifFalseValue,
                Condition = condition
            };

        }

        return ExpressionNode.Default;
    }

    private ExpressionResulType GetExpressionValue<ExpressionResulType>(Expression expression) where ExpressionResulType : class, IValue
    {
        Expression targetExpression;

        if (expression.NodeType == ExpressionType.Convert)
        {
            UnaryExpression unaryExpression = (UnaryExpression)expression;
            targetExpression = unaryExpression.Operand;
        }
        else
            targetExpression = expression;

        MemberExpression memberExpression = targetExpression as MemberExpression;
        var memberName = memberExpression?.Member?.Name;
        object? expressionResultObj = DynamicInvoke(memberExpression ?? expression);
        // Don't rename inline calculations
        // TODO : make it cleaner
        if(!string.IsNullOrWhiteSpace(memberName))
            (expressionResultObj as IName)?.Set(memberName);
        return expressionResultObj as ExpressionResulType;

        object? DynamicInvoke(Expression body) => Expression.Lambda(body).Compile().DynamicInvoke();
    }

    private string AdjustLambdaPrefix(string body) => body.Replace("() => ", "= ");

}