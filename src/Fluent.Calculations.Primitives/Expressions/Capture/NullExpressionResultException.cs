namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class NullExpressionResultException : Exception
{
    public NullExpressionResultException(string expressionBody) : base(@$"Expression ""{expressionBody}"" resulted in Null") { }
}
