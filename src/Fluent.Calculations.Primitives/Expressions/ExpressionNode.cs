using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNode
{
    public override string ToString() => $"{Body}";

    internal ExpressionNode(string body)
    {
        Body = body;
    }

    internal static ExpressionNode Default => new ExpressionNode("");

    public string Body { get; private set; } = "";

    internal ExpressionNode WithBody(string body)
    {
        Body = body;
        return this;
    }

    public virtual ArgumentsCollection Arguments { get; internal set; } = ArgumentsCollection.Empty;

    public ExpressionNode WithArguments(IValue[] arguments)
    {
        Arguments = new ArgumentsCollection(arguments);
        return this;
    }

    public ExpressionNode WithArguments(IValue a, params IValue[] b) => WithArguments(new[] { a }.Concat(b).ToArray());

}
