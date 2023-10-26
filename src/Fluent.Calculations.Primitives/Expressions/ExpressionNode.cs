namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

public  class ExpressionNode
{
    public override string ToString() => $"{Body}";

    internal static ExpressionNode None => new ExpressionNode(StringConstants.NaN, ExpressionNodeType.None) ;

    public ExpressionNode(string body, string type)
    {
        Body = body;
        Type = type;
        Arguments = ArgumentsCollection.Empty;
    }

    public string Body { get; private set; }

    public string Type { get; }

    public virtual ArgumentsCollection Arguments { get; internal set; }

    internal ExpressionNode WithBody(string body)
    {
        Body = body;
        return this;
    }

    public ExpressionNode WithArguments(IValue a, params IValue[] b) => WithArguments(new[] { a }.Concat(b));

    public ExpressionNode WithArguments(IEnumerable<IValue> arguments)
    {
        Arguments = new ArgumentsCollection(arguments);
        return this;
    }
}
