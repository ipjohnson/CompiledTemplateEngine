using CompiledTemplateEngine.Runtime.Helpers;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.String;

public abstract class BaseSingleStringTests : BaseHelperTests {
    protected async Task Evaluate(string input, string expected) {
        var defaultHelper = new DefaultHelpers();

        var templateHelperFunc = defaultHelper.GetTemplateHelperFactory(Token);

        Assert.NotNull(templateHelperFunc);
        
        var templateHelper = templateHelperFunc(null!);

        Assert.Equal(expected, await templateHelper.Execute(null!, input));
    }
}