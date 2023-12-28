namespace Fluent.Calculations.Primitives;

/// <include file="Docs.xml" path='*/EvaluationOptions/class/*'/>
public class EvaluationOptions
{
    /// <include file="Docs.xml" path='*/EvaluationOptions/Default/*'/>
    public static EvaluationOptions Default => new();

    /// <include file="Docs.xml" path='*/EvaluationOptions/AlwaysReadNamesFromExpressions/*'/>
    public bool AlwaysReadNamesFromExpressions { get; init; }

    /// <include file="Docs.xml" path='*/EvaluationOptions/Scope/*'/>
    public string Scope { get; init; } = StringConstants.NaN;
}
