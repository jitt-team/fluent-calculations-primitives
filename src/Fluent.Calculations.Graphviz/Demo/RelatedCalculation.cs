namespace Fluent.Calculations.Graphviz;
using Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
public class RelatedCalculation
{
    public Number Calculate()
    {
        var scope = Scope.CreateHere(this);

        Number
            A = Number.Of(5),
            B = Number.Of(3);

        var result = scope.Evaluate(() => A * B);

        return result;
    }
}