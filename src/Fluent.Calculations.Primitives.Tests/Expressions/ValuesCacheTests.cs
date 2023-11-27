namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class ValuesCacheTests
{
    [Fact]
    public void ValueIsAddedToCache()
    {
        Dictionary<string, IValueProvider> cacheStorage = new();
        ValuesCache cache = new(cacheStorage);
        string testValueName = "TEST-VALUE";
        cache.Add(Number.Of(1, testValueName));
        cacheStorage.Values.Should().HaveCount(1);
        cacheStorage.Values.Single().Name.Should().Be(testValueName);
    }

    [Fact]
    public void GetValueByName_IsReturned()
    {
        ValuesCache cache = new();
        string testValueName = "TEST-VALUE";
        cache.Add(Number.Of(1, testValueName));
        var value = cache.GetByName(testValueName);
        value.Name.Should().Be(testValueName);
    }

    [Fact]
    public void GetValueByKey_IsReturned()
    {
        ValuesCache cache = new();
        string testValueName = "TEST-VALUE", key = "TEST-KEY";
        cache.Add(key, Number.Of(1, testValueName));
        var value = cache.GetByKey(key);
        value.Name.Should().Be(testValueName);
    }
}
