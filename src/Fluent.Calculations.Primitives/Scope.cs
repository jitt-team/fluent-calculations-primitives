namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;

public class Scope
{
    public Scope([CallerMemberName] string name = "") => this.Name = name;

    public string Name { get; init; }

    public EvaluationContext<T> CreateContext<T>() where T : class, IValueProvider, new() =>
        new(new EvaluationOptions { AlwaysReadNamesFromExpressions = true, Scope = Name });
}