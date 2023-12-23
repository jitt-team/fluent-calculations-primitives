namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IExpression/interface/*' />
public interface IExpression
{
    IArguments Arguments { get; }
    string Body { get; }
    string Type { get; }
}