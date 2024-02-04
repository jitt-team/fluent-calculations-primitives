namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/ExpressionNode/class/*'/>
[DebuggerDisplay("Body = {FirstLineOfBody}")]
public class ExpressionNode : IExpression
{
    private ArgumentsCollection arguments;

    /// <include file="Docs.xml" path='*/ExpressionNode/ToString/*'/>
    public override string ToString() => $"{Body}";

    internal static ExpressionNode None => new(StringConstants.NaN, ExpressionNodeType.None);

    internal ExpressionNode(string body, string type)
    {
        int firstNewLineIndex = body.IndexOf(Environment.NewLine);
        FirstLineOfBody = firstNewLineIndex > 0 ? body[..firstNewLineIndex] : body;
        Body = body;
        Type = type;
        arguments = ArgumentsCollection.Empty;
    }

    /// <include file="Docs.xml" path='*/ExpressionNode/Body/*'/>
    public string Body { get; private set; }

    /// <include file="Docs.xml" path='*/ExpressionNode/Type/*'/>
    public string Type { get; }

    /// <include file="Docs.xml" path='*/ExpressionNode/Arguments/*'/>
    public virtual IArguments Arguments => arguments;

    /// <include file="Docs.xml" path='*/ExpressionNode/WithArguments/*'/>
    public ExpressionNode WithArguments(IValue first, params IValue[] other) => WithArguments(new[] { first }.Concat(other));

    /// <include file="Docs.xml" path='*/ExpressionNode/WithArguments-enumerable/*'/>
    public ExpressionNode WithArguments(IEnumerable<IValue> arguments)
    {
        this.arguments = new ArgumentsCollection(arguments);
        return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal string FirstLineOfBody { get; private set; }

    internal ExpressionNode WithBody(string body)
    {
        Body = body;
        return this;
    }

    internal void SetBody(string body) => Body = body;

    internal void AppendArgument(IValue value) => arguments.Add(value);
}