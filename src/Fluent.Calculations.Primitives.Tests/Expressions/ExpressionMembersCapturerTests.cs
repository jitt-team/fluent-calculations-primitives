namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Xunit.Abstractions;

public class ExpressionMembersCapturerTests
{
    private readonly ITestOutputHelper output;

    public ExpressionMembersCapturerTests(ITestOutputHelper output) => this.output = output;

    [Fact]
    public void test()
    {
        var capturer = new ExpressionMembersCapturer();

        Condition a = Condition.True();
        Number
            b = Number.Of(1),
            c = Number.Of(2);

        ExpressionMembersCaptureResult result = capturer.Capture(() => a ? b : c);

        output.WriteLine(result.ToString());
    }
}
