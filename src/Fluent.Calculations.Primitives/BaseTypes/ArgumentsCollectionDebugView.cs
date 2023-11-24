namespace Fluent.Calculations.Primitives.BaseTypes
{
    using System.Diagnostics;

    public class ArgumentsCollectionDebugView
    {
        private readonly ArgumentsCollection arguments;

        public ArgumentsCollectionDebugView(ArgumentsCollection arguments) => this.arguments = arguments;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IValueMetadata[] Arguments => arguments.ToArray();
    }
}