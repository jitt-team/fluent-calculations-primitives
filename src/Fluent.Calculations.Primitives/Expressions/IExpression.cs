namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IExpression
{
    IArguments Arguments { get; }
    string Body { get; }
    string Type { get; }
}