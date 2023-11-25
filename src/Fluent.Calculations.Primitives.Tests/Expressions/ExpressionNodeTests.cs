namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class ExpressionNodeTests
{
    [Fact]
    public void Test()
    {
        var node = new ExpressionNode("BODY", "TYPE").WithArguments(new[] {
            Number.Of(1, "NAME-ONE"),
            Number.Of(2, "NAME-TWO") });

        node.Body.Should().Be("BODY");
        node.Type.Should().Be("TYPE");
        node.Arguments.Should().HaveCount(2);
        node.Arguments.First().Name.Should().Be("NAME-ONE");
        node.Arguments.Last().Name.Should().Be("NAME-TWO");
    }
}
