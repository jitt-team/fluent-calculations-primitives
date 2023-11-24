using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Json
{
    public class JsonSerializationTests
    {
        [Fact]
        public void Test()
        {
            Number
                valueOne = Number.Of(1, "ValueOne"),
                valueTwo = Number.Of(2, "ValueTwo"),
                result = valueOne + valueTwo;

            string json = result.ToJson();

            // TODO, WIP
            // IValueMetadata deserialized = ValueJson.FromJson(json);

            json.Should().NotBeNullOrEmpty();
        }
    }
}
