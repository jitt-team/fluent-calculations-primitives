namespace Fluent.Calculations.Primitives;
using System.Runtime.CompilerServices;

public static class EvaluationContextExtensions
{
    public static EvaluationContext GetScope(this object obj, [CallerMemberName] string scope = StringConstants.NaN) =>
        EvaluationContext.Create(scope);

}
