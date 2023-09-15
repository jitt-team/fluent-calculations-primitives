using Fluent.Calculations.Primitives.Expressions.Capture;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class MemberAccessReflectionProviderTests
    {
        private MemberAccessReflectionProvider reflectionProvider = new MemberAccessReflectionProvider();

        [Fact]
        public void Test()
        {
            reflectionProvider.GetValue(null);


        }
    }
}
