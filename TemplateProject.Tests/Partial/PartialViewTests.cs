using DependencyModules.Testing.Attributes;
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
}