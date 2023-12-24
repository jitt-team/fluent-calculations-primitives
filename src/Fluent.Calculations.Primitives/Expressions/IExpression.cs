namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IExpression/interface/*'/>
public interface IExpression
{
    /// <include file="Docs.xml" path='*/IExpression/Arguments/*'/>
    IArguments Arguments { get; }

    /// <include file="Docs.xml" path='*/IExpression/Body/*'/>
    string Body { get; }

    /// <include file="Docs.xml" path='*/IExpression/Type/*'/>
    string Type { get; }
}