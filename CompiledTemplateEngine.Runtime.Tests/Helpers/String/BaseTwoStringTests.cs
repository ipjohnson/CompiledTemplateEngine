using CompiledTemplateEngine.Runtime.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers.String;

public abstract class BaseTwoStringTests : BaseHelperTests {
    protected async Task Evaluate(string one, string two, bool result) {
        var defaultHelper = new DefaultHelpers();

        var templateHelperFunc = defaultHelper.GetTemplateHelperFactory(Token);

        if (templateHelperFunc == null) {
            throw new Xunit.Sdk.XunitException("No template helper factory found for " + Token);
        }
        
        var serviceCollection = new ServiceCollection();
        var provider = serviceCollection.BuildServiceProvider();
        
        var templateHelper = 
            templateHelperFunc(provider);

        Assert.Equal(result, await templateHelper.Execute(null!, one, two));
    }
}