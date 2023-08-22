
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives.Expressions;

internal class ExpressionTranslator
{
    public ExpressionNode Translate<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression, [CallerArgumentExpression("expression")] string lambdaExpressionBody = "") where TExpressionResulValue : class, IValue
    {
        return TryTranslate(expression, lambdaExpressionBody)
            .WithBody(lambdaExpressionBody); // TODO: incorporate into Node
    }

    private ExpressionNode TryTranslate<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression, string lambdaExpressionBody) where TExpressionResulValue : class, IValue
    {
        // TODO : Transalte various expressions to easy to read structure
        // TODO : Extend support of more syntaxes here
        if (expression.Body.NodeType == ExpressionType.Conditional)
        {
            ConditionalExpression conditionalExpression = (ConditionalExpression)expression.Body;

            IValue ifTrueValue = GetExpressionValue<IValue>(conditionalExpression.IfTrue);
            IValue ifFalseValue = GetExpressionValue<IValue>(conditionalExpression.IfFalse);
            Condition condition = GetExpressionValue<Condition>(conditionalExpression.Test);

            var result = new ExpressionNodeConditional(lambdaExpressionBody)
            {
                IfTrue = ifTrueValue,
                IfFalse = ifFalseValue,
                Condition = condition
            };

            return result;

        }
        else if (BinaryExpressionTypes.Contains(expression.Body.NodeType))
        {
            BinaryExpression binaryExpression = (BinaryExpression)expression.Body;
            GetExpressionValue<IValue>(binaryExpression.Left);
            GetExpressionValue<IValue>(binaryExpression.Right);
        }

        return ExpressionNode.Default;
    }

    ExpressionType[] BinaryExpressionTypes = new[] {
        ExpressionType.Add,
        ExpressionType.Subtract,
        ExpressionType.Multiply,
        ExpressionType.Divide,
        ExpressionType.GreaterThan,
        ExpressionType.LessThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual
    };

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

        MemberExpression? memberExpression = targetExpression as MemberExpression;
        string? memberName = memberExpression?.Member?.Name;
        object expressionResultObj = DynamicInvoke(memberExpression ?? expression);
        // Don't rename inline calculations
        // TODO : make it cleaner
        if (!string.IsNullOrWhiteSpace(memberName))
            ((IName)expressionResultObj).Set(memberName);

        return (ExpressionResulType)expressionResultObj;

        object DynamicInvoke(Expression body) => EnsureNotNull(Expression.Lambda(body).Compile().DynamicInvoke(), body);
         
        object EnsureNotNull(object? obj, Expression body) => obj ?? throw new InvalidOperationException(@$"Expression ""{body}"" resulted in Null");
    }
}