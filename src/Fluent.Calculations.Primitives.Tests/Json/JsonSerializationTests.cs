using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.Json;
using FluentAssertions;
namespace Fluent.Calculations.Primitives.Tests.Json;

public class JsonSerializationTests
{
    [Fact]
    public void SimpleValue_Serialized_ExpectedJson()
    {
        string json = ValueJsonConverter.Serialize(Number.Of(5, "TEST-VALUE"));
        json.Should().NotBeNullOrEmpty();
        json.Should().Be(
            "{\r\n  \"Type\": \"Number\",\r\n  \"Name\": \"TEST-VALUE\",\r\n  \"Primitive\": 5,\r\n  \"PrimitiveString\": \"5.00\",\r\n  " +
            "\"Origin\": \"Constant\",\r\n  \"Expression\": {\r\n    \"Body\": \"5\",\r\n    \"Type\": \"Constant\"\r\n  }\r\n}");
    }

    [Fact]
    public void SimpleJsonValue_Deserialized_ExpectedValue()
    {
        string json = "{\r\n  \"Type\": \"Number\",\r\n  \"Name\": \"TEST-VALUE\",\r\n  \"Primitive\": 5,\r\n  \"PrimitiveString\": \"5.00\",\r\n  " +
            "\"Origin\": \"Constant\",\r\n  \"Expression\": {\r\n    \"Body\": \"5\",\r\n    \"Type\": \"Constant\"\r\n  }\r\n}";

        IValue value = ValueJsonConverter.Deserialize(json);

        value.Name.Should().Be("TEST-VALUE");
        value.Type.Should().Be("Number");
        value.Primitive.Should().Be(5);
        value.PrimitiveString.Should().Be("5.00");
        value.Origin.Should().Be(ValueOriginType.Constant);
        value.Expression.Body.Should().Be("5");
        value.Expression.Type.Should().Be("Constant");
    }

    [Fact]
    public void CalculationResult_Serialized_ExpecedJson()
    {
        Condition someCondition = Condition.True(nameof(someCondition));

        Number
            valueOne = Number.Of(1, nameof(valueOne)),
            valueTwo = Number.Of(2, nameof(valueTwo)),
            resultTwo = Result.Of(() => valueOne + valueTwo, nameof(resultTwo));

        string json = ValueJsonConverter.Serialize(resultTwo);
        json.Should().NotBeNullOrEmpty();
        json.Should().Be(
            "{\r\n  \"Type\": \"Number\",\r\n  \"Name\": \"resultTwo\",\r\n  \"Primitive\": 3,\r\n  \"PrimitiveString\": \"3.00\",\r\n  \"Origin\": \"Result\",\r\n  \"Expression\": {\r\n    " +
            "\"Arguments\": [\r\n      {\r\n        \"Type\": \"Number\",\r\n        \"Name\": \"valueOne\",\r\n        \"Primitive\": 1,\r\n        \"PrimitiveString\": \"1.00\",\r\n        \"Origin\": \"Constant\",\r\n       " +
            " \"Expression\": {\r\n          \"Body\": \"1\",\r\n          \"Type\": \"Constant\"\r\n        }\r\n      },\r\n      {\r\n        \"Type\": \"Number\",\r\n        \"Name\": \"valueTwo\",\r\n        \"Primitive\": 2,\r\n       " +
            " \"PrimitiveString\": \"2.00\",\r\n        \"Origin\": \"Constant\",\r\n        \"Expression\": {\r\n          \"Body\": \"2\",\r\n          \"Type\": \"Constant\"\r\n        }\r\n      }\r\n    ],\r\n   " +
            " \"Body\": \"valueOne \\u002B valueTwo\",\r\n    \"Type\": \"Lambda\"\r\n  }\r\n}");
    }

    [Fact]
    public void CalculationResult2_Serialized_ExpecedJson()
    {
        Condition someCondition = Condition.True(nameof(someCondition));

        Number
            valueOne = Number.Of(1, nameof(valueOne)),
            valueTwo = Number.Of(2, nameof(valueTwo)),
            resultOne = Result.Of(() => someCondition ? valueOne : valueTwo, nameof(resultOne)),
            resultTwo = Result.Of(() => resultOne + valueTwo, nameof(resultTwo));

        IValue deserialized = ValueJsonConverter.Deserialize(ValueJsonConverter.Serialize(resultTwo));       
    }

    [Fact]
    public void SumOfElements_Deserialized_ExpectedValueTree()
    {
        Values<Number> valuesCollection = Values<Number>.ListOf(() => new Number[]
        {
                Number.Of(5, "ITEM-1"),
                Number.Of(5, "ITEM-2")
        }, nameof(valuesCollection));

        Number result = Result.Of(() => valuesCollection.Sum(), nameof(result));

        IValue deserialized = ValueJsonConverter.Deserialize(ValueJsonConverter.Serialize(result));

        deserialized.Name.Should().Be(nameof(result));
        deserialized.Primitive.Should().Be(10);
        deserialized.Expression.Body.Should().Be("valuesCollection.Sum()");
        deserialized.Expression.Arguments.Should().HaveCount(1);

        var resultArgument = deserialized.Expression.Arguments.Single();
        resultArgument.Name.Should().Be(nameof(valuesCollection));
        resultArgument.Expression.Arguments.Should().HaveCount(2);
        resultArgument.Expression.Arguments.First().Name.Should().Be("ITEM-1");
        resultArgument.Expression.Arguments.Last().Name.Should().Be("ITEM-2");
    }
}