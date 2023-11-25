using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Json;
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

            string json = JsonValueConverter.ToJson(resultTwo);

            IValueMetadata deserialized = JsonValueConverter.ToValue(json);

            json.Should().NotBeNullOrEmpty();
        }
    }
}
