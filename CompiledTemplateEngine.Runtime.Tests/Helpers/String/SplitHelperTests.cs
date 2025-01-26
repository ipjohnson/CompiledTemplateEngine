using CompiledTemplateEngine.Runtime.Helpers;
using CompiledTemplateEngine.Runtime.Helpers.String;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.String;

public class SplitHelperTests : BaseHelperTests {
    [Fact]
    public async Task SplitStringWithString() {
        var helper = GetHelper();

        var result = await helper.Execute(GetExecutionContext(), "string string", " ");

        Assert.NotNull(result);
        Assert.Equal(new[] { "string", "string" }, result);
    }

    [Fact]
    public async Task SplitStringWithInt() {
        var helper = GetHelper();

        var result = await helper.Execute(GetExecutionContext(), "string1string", 1);

        Assert.NotNull(result);
        Assert.Equal(new[] { "string", "string" }, result);
    }

    protected override Type TemplateHelperType => typeof(SplitHelper);
    protected override string Token => DefaultHelpers.StringHelperToken.Split;
}