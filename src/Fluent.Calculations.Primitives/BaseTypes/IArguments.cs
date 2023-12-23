namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='docs/members[@name="IArguments"]/class/*' />
public interface IArguments : IEnumerable<IValue>
{
    public int Count { get; }
}