namespace Fluent.Calculations.Primitives;

public class ArgumentsList : List<IValue>
{
    private ArgumentsList()
    {

    }

    internal ArgumentsList(IEnumerable<IValue> collection) : base(collection)
    {
    }

    internal static ArgumentsList Empty => new ArgumentsList();

    internal static ArgumentsList CreateFrom(IValue[] arguments) => new ArgumentsList(arguments);

}
