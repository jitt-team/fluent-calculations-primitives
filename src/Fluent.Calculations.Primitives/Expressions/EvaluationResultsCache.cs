namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class EvaluationResultsCache : IEvaluationResultsCache
{
    private readonly Dictionary<string, IValue> cache = new Dictionary<string, IValue>();

    public void Add(IValue value) => cache.Add(value.Name, value);

    public void Add(string name, IValue value) => cache.Add(name, value);

    public bool ContainsKey(string name) => cache.ContainsKey(name);

    public bool TryGetValue(string lambdaExpressionBodyAdjusted, out IValue? cachedValue) =>
        cache.TryGetValue(lambdaExpressionBodyAdjusted, out cachedValue);

    public IValue GetByKey(string name) => cache[name];

    public bool ContainsName(string name) => cache.Values.Any(value => value.Name == name);

    public IValue GetByName(string name) => cache.Values.Single(value => value.Name == name);
}
