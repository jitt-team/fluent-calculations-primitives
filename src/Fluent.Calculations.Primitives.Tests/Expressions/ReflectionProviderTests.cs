using Fluent.Calculations.Primitives.Expressions.Capture;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class ReflectionProviderTests
    {
        private ReflectionProvider reflectionProvider = new ReflectionProvider();

        [Fact]
        public void Test()
        {
            reflectionProvider.GetValue(null);


        }
    }
}
