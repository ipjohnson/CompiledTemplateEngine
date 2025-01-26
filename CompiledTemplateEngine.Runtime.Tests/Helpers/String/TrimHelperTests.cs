using CompiledTemplateEngine.Runtime.Tests.Helpers.String;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Helpers.String;

public class TrimHelperTests : BaseSingleStringTests {
    [Theory]
    [InlineData(" Hello", "Hello")]
    [InlineData("Hello ", "Hello")]
    [InlineData("Hello", "Hello")]
    public async Task TrimLogicTests(string input, string expected) {
        await Evaluate(input, expected);
    }

    protected override Type TemplateHelperType => typeof(TrimHelper);

    protected override string Token => DefaultHelpers.StringHelperToken.Trim;
}