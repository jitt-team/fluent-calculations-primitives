namespace Fluent.Calculations.Primitives;
using System.Runtime.CompilerServices;

/// <include file="IntelliSense.xml" path='*/EvaluationScopeExtensions/class/*'/>
public static class EvaluationScopeExtensions
{
    public static EvaluationScope GetScope(this object obj, [CallerMemberName] string scope = StringConstants.NaN) =>
        EvaluationScope.Create($"{obj.GetType().Name}.{scope}");

}
