namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="IOrigin"]/interface/*' />
internal interface IOrigin
{
    bool IsSet { get; }

    IValueProvider AsResult();

    void MarkAsParameter(string name, string scope);
}