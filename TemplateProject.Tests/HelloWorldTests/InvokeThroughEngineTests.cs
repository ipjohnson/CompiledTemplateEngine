using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.xUnit.Attributes;
using TemplateProject.Models;
using Xunit;

namespace TemplateProject.Tests.HelloWorldTests;

public class InvokeThroughEngineTests {
    
    [ModuleTest]
    public async Task InvokeFromEngine(ITemplateExecutionService templateExecutionService, IServiceProvider serviceProvider) {
        var result = await templateExecutionService.Execute(
            "hello-world", 
            new HelloWorldModel("Hello", "World"));
        
        Assert.NotEmpty(result);
        Assert.Contains("Hello World", result);
        
    }

    [ModuleTest]
    public async Task InvokeUsingTemplateInvoker(TemplatesModule.IInvoker templateInvoker) {
        var result = await templateInvoker.HelloWorld(new HelloWorldModel("Hello", "World"));
        
        Assert.NotEmpty(result);
        Assert.Contains("Hello World", result);
    }
}