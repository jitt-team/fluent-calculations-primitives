﻿namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

public interface IMultiPartExpressionMemberExtractor
{
    Expression[] ExtractBinaryExpressionMembers(BinaryExpression expression);

    Expression[] ExtractConditionalExpressionMembers(ConditionalExpression expression);

    bool IsBinaryExpression(ExpressionType expressionType);
}