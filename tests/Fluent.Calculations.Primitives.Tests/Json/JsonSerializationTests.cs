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
        Number testValue = Number.Of(5, "TEST-SCOPE", nameof(testValue));
        string json = ValueJsonConverter.Serialize(testValue, writeIndented: false);
        json.Should().NotBeNullOrEmpty();
        json.Should().Be(@"{""Type"":""Number"",""Name"":""testValue"",""Scope"":""TEST-SCOPE"",""Primitive"":5,""PrimitiveString"":""5.00"",""Origin"":""Constant"",""Expression"":{""Body"":""5"",""Type"":""Constant""}}");
    }

    [Fact]
    public void SimpleJsonValue_Deserialized_ExpectedValue()
    {
        string json = @"{""Type"":""Number"",""Name"":""TEST-VALUE"",""Scope"":""TEST-SCOPE"",""Primitive"":5,""PrimitiveString"":""5.00"",""Origin"":""Constant"",""Expression"":{""Body"":""5"",""Type"":""Constant""}}";

        IValue value = ValueJsonConverter.Deserialize(json);

        value.Scope.Should().Be("TEST-SCOPE");
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
        string scope = "TEST-SCOPE";

        Condition someCondition = Condition.True(scope, nameof(someCondition));

        Number
            valueOne = Number.Of(1, scope, nameof(valueOne)),
            valueTwo = Number.Of(2, scope,nameof(valueTwo)),
            resultTwo = Result.OfWithScope(() => valueOne + valueTwo, scope, nameof(resultTwo));

        string json = ValueJsonConverter.Serialize(resultTwo, writeIndented: false);
        json.Should().NotBeNullOrEmpty();
        json.Should().Be(
            @"{""Type"":""Number"",""Name"":""resultTwo"",""Scope"":""TEST-SCOPE"",""Primitive"":3,""PrimitiveString"":""3.00"",""Origin"":""Result"",""Expression"":{""Arguments"":[{""Type"":""Number""," +
            @"""Name"":""valueOne"",""Scope"":""TEST-SCOPE"",""Primitive"":1,""PrimitiveString"":""1.00"",""Origin"":""Constant"",""Expression"":{""Body"":""1"",""Type"":""Constant""}},{""Type"":""Number""," +
            @"""Name"":""valueTwo"",""Scope"":""TEST-SCOPE"",""Primitive"":2,""PrimitiveString"":""2.00"",""Origin"":""Constant"",""Expression"":{""Body"":""2"",""Type"":""Constant""}}],""Body"":""valueOne \u002B valueTwo"",""Type"":""Lambda""}}");
    }

    [Fact]
    public void CalculationResult_SameAfterDeserialization()
    {
        string scope = "TEST-SCOPE";

        Condition someCondition = Condition.True(nameof(someCondition));

        Number
            valueOne = Number.Of(1, scope, nameof(valueOne)),
            valueTwo = Number.Of(2, scope, nameof(valueTwo)),
            resultOne = Result.OfWithScope(() => someCondition ? valueOne : valueTwo, scope, nameof(resultOne));

        Number finalResult = Result.OfWithScope(() => resultOne + valueTwo, scope, nameof(finalResult));

        IValue deserialized = ValueJsonConverter.Deserialize(ValueJsonConverter.Serialize(finalResult));

        deserialized.Scope.Should().Be(scope);
        deserialized.Primitive.Should().Be(finalResult.Primitive);
        deserialized.Name.Should().Be(finalResult.Name);
        deserialized.Expression.Body.Should().Be(finalResult.Expression.Body);
        deserialized.Expression.Type.Should().Be(finalResult.Expression.Type);
        deserialized.Expression.Arguments.Count.Should().Be(finalResult.Expression.Arguments.Count);
        deserialized.Expression.Arguments.First().Scope.Should().Be(scope);
    }

    [Fact]
    public void SumOfElements_Deserialized_ExpectedValueTree()
    {
        var numbers = new Number[]
        {
                Number.Of(5, "ITEM-1"),
                Number.Of(5, "ITEM-2")
        };

        Values<Number> valuesCollection = Values<Number>.ListOf(() => numbers, nameof(valuesCollection));

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
