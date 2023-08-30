namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class ExpressionResultCache
{
    private readonly Dictionary<string, IValue> cache = new Dictionary<string, IValue>();

    internal void Add(IValue value) => cache.Add(value.Name, value);

    internal void Add(string name, IValue value) => cache.Add(name, value);

    internal bool ContainsKey(string name) => cache.ContainsKey(name);

    internal bool TryGetValue(string lambdaExpressionBodyAdjusted, out IValue? cachedValue) =>
        cache.TryGetValue(lambdaExpressionBodyAdjusted, out cachedValue);

    internal IValue GetByKey(string name) => cache[name];
}
