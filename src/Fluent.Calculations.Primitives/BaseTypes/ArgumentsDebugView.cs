namespace Fluent.Calculations.Primitives.BaseTypes
{
    using System.Diagnostics;

    public class ArgumentsDebugView
    {
        private readonly IArguments arguments;

        public ArgumentsDebugView(IArguments arguments) => this.arguments = arguments;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IValue[] Arguments => arguments.ToArray();
    }
}