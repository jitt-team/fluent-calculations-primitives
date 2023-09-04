﻿namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeComparison : ExpressionNode
{
    internal static ExpressionNode Create(string operationBody) => new ExpressionNodeBinaryExpression(operationBody);

    internal ExpressionNodeComparison(string body) : base(body, ExpressionNodeType.Comparision)
    {

    }
}
