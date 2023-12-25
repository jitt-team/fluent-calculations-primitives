namespace Fluent.Calculations.Primitives;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/EvaluationScopeExtensions/class/*'/>
public static class EvaluationScopeExtensions
{
    /// <include file="Docs.xml" path='*/EvaluationScopeExtensions/GetScope/*'/>
    public static EvaluationScope GetScope(this object obj, [CallerMemberName] string scope = StringConstants.NaN) =>
        EvaluationScope.Create($"{obj.GetType().Name}.{scope}");
}
