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
            Expression<Func<Number>> testEpression = () => testValue;
            IValue result = ValueExpressionInvoker.DynamicInvoke(testEpression.Body);
            result.Name.Should().Be(testValue.Name);
        }

        [Fact]
        public void NullValue_Throws()
        {
            Number testValue = null;
            Expression<Func<Number>> testEpression = () => testValue;
            Assert.Throws<NullExpressionResultException>(() => ValueExpressionInvoker.DynamicInvoke(testEpression.Body));
        }
    }
}
