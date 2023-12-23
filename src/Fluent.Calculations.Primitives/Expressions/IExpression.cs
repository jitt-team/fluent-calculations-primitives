namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='docs/members[@name="IExpression"]/interface/*' />
public interface IExpression
{
    IArguments Arguments { get; }
    string Body { get; }
    string Type { get; }
}