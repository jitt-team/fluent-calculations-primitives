﻿namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal class MemberExpressionsCapturer : IMemberExpressionsCapturer
{
    private readonly IMultiPartExpressionMemberExtractor _expressionMemberExtractor;

    public MemberExpressionsCapturer() : this(new MultiPartExpressionMemberExtractor()) { }

    public MemberExpressionsCapturer(IMultiPartExpressionMemberExtractor expressionMemberExtractor) =>
        _expressionMemberExtractor = expressionMemberExtractor;

    public List<MemberExpression> Capture(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Lambda)
            return Capture(((LambdaExpression)expression).Body);
        else if (expression.NodeType == ExpressionType.Convert)
            return Capture(((UnaryExpression)expression).Operand);
        else if (_expressionMemberExtractor.IsBinaryExpression(expression.NodeType))
            return CaptureMultiple(_expressionMemberExtractor.ExtractBinaryExpressionMembers((BinaryExpression)expression));
        else if (expression.NodeType == ExpressionType.Conditional)
            return CaptureMultiple(_expressionMemberExtractor.ExtractConditionalExpressionMembers((ConditionalExpression)expression));
        else if (expression.NodeType == ExpressionType.MemberAccess)
            return new List<MemberExpression> { (MemberExpression)expression };
        throw new NotImplementedException($"Expression type {expression.NodeType} body {expression} not implemented");
    }

    private List<MemberExpression> CaptureMultiple(Expression[] expressions) => expressions.SelectMany(Capture).ToList();
}

public class MyVisitor : ExpressionVisitor
{ 
    private readonly List<MemberExpression> memberExpressions = new List<MemberExpression>();

    public List<MemberExpression> Capture(Expression expression)
    {

        return new List<MemberExpression>();
    }

    protected override Expression VisitLambda<IValue>(Expression<IValue> node)
    {
        return base.VisitLambda(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        return base.VisitBinary(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        return base.VisitConditional(node);
    }

}