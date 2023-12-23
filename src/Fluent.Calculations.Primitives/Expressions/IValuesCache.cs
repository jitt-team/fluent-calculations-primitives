namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IValuesCache/interface/*' />
internal interface IValuesCache
{
    void Add(IValueProvider value);

    void Add(string key, IValueProvider value);

    bool ContainsKey(string key);

    bool ContainsName(string name);

    IValueProvider GetByKey(string key);

    IValueProvider GetByName(string name);

    bool TryGetValue(string key, out IValueProvider? cachedValue);

    void Clear();
}