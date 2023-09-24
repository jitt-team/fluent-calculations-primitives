namespace Fluent.Calculations.Primitives.BaseTypes
{
    internal interface IOrigin
    {
        bool IsSet { get; }

        IValue AsResult();

        void MarkAsParameter(string name);
    }
}