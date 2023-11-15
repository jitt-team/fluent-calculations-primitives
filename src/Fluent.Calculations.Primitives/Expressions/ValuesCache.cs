namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class ValuesCache : IValuesCache
{
    private readonly IDictionary<string, IValue> cache;

    public ValuesCache() : this(new Dictionary<string, IValue>(100)) { }

    public ValuesCache(IDictionary<string, IValue> cache) => this.cache = cache;

    public void Add(IValue value) => cache.Add(value.Name, value);

    public void Add(string key, IValue value) => cache.Add(key, value);

    public bool ContainsKey(string key) => cache.ContainsKey(key);

    public bool TryGetValue(string key, out IValue? cachedValue) => cache.TryGetValue(key, out cachedValue);

    public IValue GetByKey(string key) => cache[key];

    public bool ContainsName(string name) => cache.Values.Any(value => value.Name == name);

    public IValue GetByName(string name) => cache.Values.Single(value => value.Name == name);

    public void Clear() => cache.Clear();
}
