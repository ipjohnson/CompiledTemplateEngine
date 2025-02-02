﻿using CompiledTemplateEngine.Runtime.Helpers;
using CompiledTemplateEngine.Runtime.Helpers.Collection;
using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.xUnit.Attributes;
using NSubstitute;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.Collection;

public class LookupHelperTests : BaseHelperTests {
    private readonly ITemplateExecutionContext _mockExecutionContext =
        Substitute.For<ITemplateExecutionContext>();

    [ModuleTest]
    public async Task ArrayLookup(LookupHelper lookupHelper) {
        var result = await lookupHelper.Execute(_mockExecutionContext, new[] { 1, 2, 3 }, 2);

        Assert.NotNull(result);
        Assert.Equal(3, result);
    }

    [ModuleTest]
    public async Task ArrayLookupIndexOutOfRange(LookupHelper lookupHelper) {
        var result = await lookupHelper.Execute(_mockExecutionContext, new[] { 1, 2, 3 }, -1);

        Assert.Null(result);

        result = await lookupHelper.Execute(_mockExecutionContext, new[] { 1, 2, 3 }, 3);

        Assert.Null(result);
    }

    [ModuleTest]
    public async Task ListLookup(LookupHelper lookupHelper) {
        var result = await lookupHelper.Execute(_mockExecutionContext, new List<int> { 1, 2, 3 }, 2);

        Assert.NotNull(result);
        Assert.Equal(3, result);
    }

    [ModuleTest]
    public async Task ListLookupIndexOutOfRange(LookupHelper lookupHelper) {
        var result = await lookupHelper.Execute(_mockExecutionContext, new List<int> { 1, 2, 3 }, -1);

        Assert.Null(result);

        result = await lookupHelper.Execute(_mockExecutionContext, new List<int> { 1, 2, 3 }, 3);

        Assert.Null(result);
    }

    [ModuleTest]
    public async Task DictionaryLookup(LookupHelper lookupHelper) {
        var dictionary = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 }, { "key3", 3 } };

        var result = await lookupHelper.Execute(_mockExecutionContext, dictionary, "key3");

        Assert.NotNull(result);
        Assert.Equal(3, result);
    }

    [ModuleTest]
    public async Task DictionaryLookupNotFound(LookupHelper lookupHelper) {
        var dictionary = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 }, { "key3", 3 } };

        var result = await lookupHelper.Execute(_mockExecutionContext, dictionary, "key4");

        Assert.Null(result);
    }

    protected override Type TemplateHelperType => typeof(LookupHelper);

    protected override string Token => DefaultHelpers.CollectionHelperTokens.Lookup;
}