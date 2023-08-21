using Fluent.Calculations.Primitives.Expressions;
using Xunit.Abstractions;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class ExpressionTranslatorTests
    {
        private readonly ITestOutputHelper output;

        public ExpressionTranslatorTests(ITestOutputHelper output) => this.output = output;

        [Fact]
        public void test()
        {
            var translator = new ExpressionTranslator();

            Condition a = Condition.True();
            Number
                b = Number.Of(1),
                c = Number.Of(2);

            ExpressionNode result = translator.Translate(() => a ? b : c);

            output.WriteLine(result.ToString());
        }
    }
}
