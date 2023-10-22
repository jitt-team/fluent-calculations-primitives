using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.BaseTypes
{
    public class ValueTests
    {
        [Fact]
        public void Test()
        {
            true.Should().BeTrue();
        }
    }
}
