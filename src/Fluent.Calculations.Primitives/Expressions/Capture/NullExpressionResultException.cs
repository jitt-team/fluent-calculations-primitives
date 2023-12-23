﻿namespace Fluent.Calculations.Primitives.Expressions.Capture;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="NullExpressionResultException"]/exception/*' />
public class NullExpressionResultException : Exception
{
    public NullExpressionResultException(string expressionBody) : base(@$"Expression ""{expressionBody}"" resulted in Null") { }
}
