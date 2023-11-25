using System.Diagnostics;

namespace Fluent.Calculations.Primitives.BaseTypes
{
    public interface IArguments : IEnumerable<IValueMetadata>
    {
        public int Count { get; }
    }
}