namespace Fluent.Calculations.Primitives.BaseTypes;
public interface IValue : IValueMetadata
{
    IValue MakeOfThisType(MakeValueArgs args);

    IValue GetDefault();

    string ToJson();
}
