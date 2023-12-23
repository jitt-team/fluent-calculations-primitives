namespace Fluent.Calculations.Primitives.BaseTypes;

internal interface IOrigin
{
    bool IsSet { get; }

    IValueProvider AsResult();

    void MarkAsParameter(string name, string scope);
}