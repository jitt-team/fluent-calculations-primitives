namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

public interface IValuesCache
{
    void Add(IValue value);
    void Add(string key, IValue value);
    bool ContainsKey(string key);
    bool ContainsName(string name);
    IValue GetByKey(string key);
    IValue GetByName(string name);
    bool TryGetValue(string key, out IValue? cachedValue);
}