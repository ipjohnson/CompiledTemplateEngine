﻿using CompiledTemplateEngine.Runtime.Helpers;
using CompiledTemplateEngine.Runtime.Helpers.String;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.String;

public class ContainsHelperTests : BaseTwoStringTests {
    [Theory]
    [InlineData("Hello", "llo", true)]
    [InlineData("Hello", "ell", true)]
    [InlineData("Hello", "He", true)]
    [InlineData("Hello", "", false)]
    [InlineData("", "llo", false)]
    [InlineData("", "", false)]
    public Task EndsWithLogicTests(string one, string two, bool result) {
        return Evaluate(one, two, result);
    }

    protected override Type TemplateHelperType => typeof(ContainsHelper);

    protected override string Token => DefaultHelpers.StringHelperToken.Contains;
}