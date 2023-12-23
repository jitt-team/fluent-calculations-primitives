namespace Fluent.Calculations.Primitives.Expressions.Capture;

/// <include file="Docs.xml" path='*/NullExpressionResultException/exception/*' />
public class NullExpressionResultException : Exception
{
    public NullExpressionResultException(string expressionBody) : base(@$"Expression ""{expressionBody}"" resulted in Null") { }
}
