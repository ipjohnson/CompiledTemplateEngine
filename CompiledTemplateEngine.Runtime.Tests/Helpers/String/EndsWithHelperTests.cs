﻿using CompiledTemplateEngine.Runtime.Helpers;
using CompiledTemplateEngine.Runtime.Helpers.String;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.String;

public class EndsWithHelperTests : BaseTwoStringTests {
    [Theory]
    [InlineData("Hello", "llo", true)]
    [InlineData("Hello", "rld", false)]
    [InlineData("Hello", "", false)]
    [InlineData("", "llo", false)]
    [InlineData("", "", false)]
    public Task EndsWithLogicTests(string one, string two, bool result) {
        return Evaluate(one, two, result);
    }

    protected override string Token => DefaultHelpers.StringHelperToken.EndsWith;

    protected override Type TemplateHelperType => typeof(EndsWithHelper);
}