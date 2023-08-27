namespace Fluent.Calculations.Primitives
{
    internal class ExpressionResultCache
    {
        private readonly Dictionary<string, IValue> evalueatedExpressionResults = new Dictionary<string, IValue>();

        internal void Add(IValue value) => evalueatedExpressionResults.Add(value.Name, value);

        internal void Add(string name, IValue value) => evalueatedExpressionResults.Add(name, value);

        internal bool ContainsKey(string name) => evalueatedExpressionResults.ContainsKey(name);

        internal bool TryGetValue(string lambdaExpressionBodyAdjusted, out IValue? cachedValue) =>
            evalueatedExpressionResults.TryGetValue(lambdaExpressionBodyAdjusted, out cachedValue);

        internal IValue GetByKey(string name) => evalueatedExpressionResults[name];
    }
}
