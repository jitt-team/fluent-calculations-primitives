using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions
{
    internal interface IValueArgumentsSelector
    {
        IValue[] Select(IValue value);
    }
}