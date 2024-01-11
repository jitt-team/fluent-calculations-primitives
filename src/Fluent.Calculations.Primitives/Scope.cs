namespace Fluent.Calculations.Primitives;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/Scope/class/*'/>
public static class Scope
{
    /// <include file="Docs.xml" path='*/Scope/CreateHere/*'/>
    public static EvaluationScope CreateHere(object obj, [CallerMemberName] string scope = StringConstants.NaN) =>
        EvaluationScope.Create($"{obj.GetType().Name}.{scope}");
}
