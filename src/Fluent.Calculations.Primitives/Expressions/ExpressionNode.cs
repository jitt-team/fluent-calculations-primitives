namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="ExpressionNode"]/class/*' />
[DebuggerDisplay("Body = {FirstLineOfBody}")]
public class ExpressionNode : IExpression
{
    private ArgumentsCollection arguments;

    public override string ToString() => $"{Body}";

    internal static ExpressionNode None => new(StringConstants.NaN, ExpressionNodeType.None);

    public ExpressionNode(string body, string type)
    {
        int firstNewLineIndex = body.IndexOf(Environment.NewLine);
        FirstLineOfBody = firstNewLineIndex > 0 ? body[..firstNewLineIndex] : body;
        Body = body;
        Type = type;
        arguments = ArgumentsCollection.Empty;
    }

    public string Body { get; private set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal string FirstLineOfBody { get; private set; }

    public string Type { get; }

    public virtual IArguments Arguments => arguments;

    internal ExpressionNode WithBody(string body)
    {
        Body = body;
        return this;
    }

    public ExpressionNode WithArguments(IValue a, params IValue[] b) => WithArguments(new[] { a }.Concat(b));

    public ExpressionNode WithArguments(IEnumerable<IValue> arguments)
    {
        this.arguments = new ArgumentsCollection(arguments);
        return this;
    }

    internal void SetBody(string body) => Body = body;

    internal void AppendArgument(IValue value) => arguments.Add(value);
}