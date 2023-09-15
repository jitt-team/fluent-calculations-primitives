using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Expressions
{
    public class ValueExpressionInvokerTests
    {
        [Fact]
        public void HasValue_ReturnsResult()
        {
            Number testValue = Number.Of(1, "TEST-VALUE");
            Expression<Func<Number>> testExpression = () => testValue;
            IValue result = MemberAccessReflectionProvider.DynamicInvoke(testExpression.Body);
            result.Name.Should().Be(testValue.Name);
        }

        [Fact]
        public void NullValue_Throws()
        {
            Number testValue = null;
            Expression<Func<Number>> testExpression = () => testValue;
            Assert.Throws<NullExpressionResultException>(() => MemberAccessReflectionProvider.DynamicInvoke(testExpression.Body));
        }
    }
}
