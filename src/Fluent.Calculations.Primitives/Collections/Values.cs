namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;

public class Values<T> : IEnumerable<IValue>
{
    private readonly List<IValue> numbers = new List<IValue>();

    public void Add(IValue number) => numbers.Add(number);

    public IEnumerator<IValue> GetEnumerator() => numbers.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => numbers.GetEnumerator();
}

