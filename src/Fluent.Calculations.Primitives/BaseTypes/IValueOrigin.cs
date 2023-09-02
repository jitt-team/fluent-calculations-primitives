namespace Fluent.Calculations.Primitives.BaseTypes
{
    internal interface IValueOrigin
    {
        IValue MarkAsEndResult();
        IValue MarkAsInput();
    }
}