using DependencyModules.Runtime;
using DependencyModules.xUnit.Attributes;
using Microsoft.Extensions.DependencyInjection;
using TemplateProject.Models;
using Xunit;

namespace TemplateProject.Tests.Partial;

public class PartialViewTests {
    [ModuleTest]
    public async Task InvokeRoot(Templates.IInvoker invoker) {
        var result = await invoker.RootTemplate(new HelloWorldModel("Hello", "World"));

        Assert.NotNull(result);
        Assert.Contains("<div>Hello World</div>", result);
    }
    
    [ModuleTest]
    public async Task InvokePartial(Templates.IInvoker invoker) {
        var result = await invoker.PartialTemplate(new HelloWorldModel("Hello", "World"));

        Assert.NotNull(result);
        Assert.Equal("\n<div>Hello World</div>", result);
    }

    [Fact]
    public async Task Test() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddModule(new Templates());
        var provider = serviceCollection.BuildServiceProvider();

        var invoker = provider.GetRequiredService<Templates.IInvoker>();
        
        var output = await invoker.RootTemplate(new HelloWorldModel("Hello", "World"));
        
        Assert.Contains("Hello World", output);
    }
}