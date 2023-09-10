namespace Fluent.Calculations.Primitives.BaseTypes
{
    internal interface IOrigin
    {
        IValue MarkAsEndResult();
        IValue MarkAsInput();
    }
}