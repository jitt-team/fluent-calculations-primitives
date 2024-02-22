namespace Fluent.Calculations.Primitives.Expressions.Capture;

/// <include file="Docs.xml" path='*/NullExpressionResultException/exception/*'/>
public class NullExpressionResultException : Exception
{
    internal NullExpressionResultException(string expressionBody) : base(@$"Expression ""{expressionBody}"" resulted in Null") { }
}
