namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="ValuesCache"]/class/*' />
internal class ValuesCache : IValuesCache
{
    private readonly IDictionary<string, IValueProvider> cache;

    public ValuesCache() : this(new Dictionary<string, IValueProvider>(100)) { }

    public ValuesCache(IDictionary<string, IValueProvider> cache) => this.cache = cache;

    public void Add(IValueProvider value) => cache.Add(value.Name, value);

    public void Add(string key, IValueProvider value) => cache.Add(key, value);

    public bool ContainsKey(string key) => cache.ContainsKey(key);

    public bool TryGetValue(string key, out IValueProvider? cachedValue) => cache.TryGetValue(key, out cachedValue);

    public IValueProvider GetByKey(string key) => cache[key];

    public bool ContainsName(string name) => cache.Values.Any(value => value.Name == name);

    public IValueProvider GetByName(string name) => cache.Values.Single(value => value.Name == name);

    public void Clear() => cache.Clear();
}
