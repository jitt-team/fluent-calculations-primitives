using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions
{
    internal interface IEvaluationResultsCache
    {
        void Add(IValue value);
        void Add(string name, IValue value);
        bool ContainsKey(string name);
        bool ContainsName(string name);
        IValue GetByKey(string name);
        IValue GetByName(string name);
        bool TryGetValue(string lambdaExpressionBodyAdjusted, out IValue? cachedValue);
    }
}