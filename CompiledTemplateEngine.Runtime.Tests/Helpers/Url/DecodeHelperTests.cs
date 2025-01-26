using CompiledTemplateEngine.Runtime.Tests.Helpers;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Helpers.Url;

public class DecodeHelperTests : BaseHelperTests {
    [Fact]
    public async Task DecodeUrlTest() {
        var helper = GetHelper();

        var result = await helper.Execute(GetExecutionContext(), "https%3a%2f%2fwww.google.com%2f");

        Assert.NotNull(result);
        Assert.Equal("https://www.google.com/", result);
    }

    protected override Type TemplateHelperType => typeof(DecodeHelper);

    protected override string Token => DefaultHelpers.UrlHelperTokens.Decode;
}