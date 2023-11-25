using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.Tests.Collections;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Json
{
    public class JsonSerializationTests
    {
        [Fact]
        public void Test()
        {
            Condition someCondition = Condition.True(nameof(someCondition));

            Number
                valueOne = Number.Of(1, nameof(valueOne)),
                valueTwo = Number.Of(2, nameof(valueTwo)),
                resultOne = Result.Of(() => someCondition ? valueOne : valueTwo, nameof(resultOne)),
                resultTwo = Result.Of(() => resultOne + valueTwo, nameof(resultTwo));

            string json = ValueJsonConverter.ToJson(resultTwo);

            IValue deserialized = ValueJsonConverter.ToValue(json);

            json.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Test2()
        {
            Values<Number> valuesCollection = Values<Number>.SumOf(() => new Number[] {
                    Number.Of(5, "ITEM-1"),
                    Number.Of(5, "ITEM-2")
            }, nameof(valuesCollection));

            Number valueOne = Number.Of(1, nameof(valueOne));

            Number result = Result.Of(() => valueOne + valuesCollection.Sum(), nameof(result));

            string json = ValueJsonConverter.ToJson(result);

            IValue deserialized = ValueJsonConverter.ToValue(json);

            json.Should().NotBeNullOrEmpty();
        }
    }
}
