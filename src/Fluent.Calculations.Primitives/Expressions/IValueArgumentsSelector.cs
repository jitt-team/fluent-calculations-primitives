using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions
{
    internal interface IValueArgumentsSelector
    {
        IValue[] SelectArguments(IValue value);
    }
}