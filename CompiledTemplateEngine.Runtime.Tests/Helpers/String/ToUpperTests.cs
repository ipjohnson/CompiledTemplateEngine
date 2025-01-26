﻿using CompiledTemplateEngine.Runtime.Tests.Helpers.String;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Helpers.String;

public class ToUpperTests : BaseSingleStringTests {
    [Theory]
    [InlineData("Hello", "HELLO")]
    [InlineData("hello world", "HELLO WORLD")]
    public async Task ToUpperLogicTests(string input, string expected) {
        await Evaluate(input, expected);
    }

    protected override Type TemplateHelperType => typeof(ToUpperHelper);

    protected override string Token => DefaultHelpers.StringHelperToken.ToUpper;
}