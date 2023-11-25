namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

[DebuggerDisplay("Body = {Body}")]
public class ExpressionNode : IExpression
{
    private ArgumentsCollection arguments;

    public override string ToString() => $"{Body}";

    internal static ExpressionNode None => new ExpressionNode(StringConstants.NaN, ExpressionNodeType.None);

    public ExpressionNode(string body, string type)
    {
        Body = body;
        Type = type;
        arguments = ArgumentsCollection.Empty;
    }

    public string Body { get; private set; }

    public string Type { get; }

    public virtual IArguments Arguments => arguments;

    internal ExpressionNode WithBody(string body)
    {
        Body = body;
        return this;
    }

    public ExpressionNode WithArguments(IValueMetadata a, params IValueMetadata[] b) => WithArguments(new[] { a }.Concat(b));

    public ExpressionNode WithArguments(IEnumerable<IValueMetadata> arguments)
    {
        this.arguments = new ArgumentsCollection(arguments);
        return this;
    }

    internal void SetBody(string body) => Body = body;

    internal void AppendArgument(IValueMetadata value) => arguments.Add(value);
}