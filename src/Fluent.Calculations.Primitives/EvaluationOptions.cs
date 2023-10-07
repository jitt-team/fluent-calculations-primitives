namespace Fluent.Calculations.Primitives;

public class EvaluationOptions
{
    public static EvaluationOptions Default = new EvaluationOptions();

    public bool AlwaysReadNamesFromExpressions { get; set; }
}
