﻿namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class ValuesCache(IDictionary<string, IValueProvider> cache) : IValuesCache
{
    private readonly IDictionary<string, IValueProvider> cache = cache;

    public ValuesCache() : this(new Dictionary<string, IValueProvider>(100)) { }

    public void Add(IValueProvider value) => cache.Add(value.Name, value);

    public void Add(string key, IValueProvider value) => cache.Add(key, value);

    public bool ContainsKey(string key) => cache.ContainsKey(key);

    public bool TryGetValue(string key, out IValueProvider? cachedValue) => cache.TryGetValue(key, out cachedValue);

    public IValueProvider GetByKey(string key) => cache[key];

    public bool ContainsName(string name) => cache.Values.Any(value => value.Name == name);

    public IValueProvider GetByName(string name) => cache.Values.Single(value => value.Name == name);

    public void Clear() => cache.Clear();
}
