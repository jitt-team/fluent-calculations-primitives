namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal interface IValueArgumentsSelector
{
    IValue[] Select(IValue value);
}