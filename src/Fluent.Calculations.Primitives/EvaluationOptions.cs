namespace Fluent.Calculations.Primitives;

public class EvaluationOptions
{
    public static EvaluationOptions Default => new();

    public bool AlwaysReadNamesFromExpressions { get; init; }

    public string Scope { get; init; } = StringConstants.NaN;
}
