# CompiledTemplateEngine

A lightweight template engine that leverages source generation to create compiled templates that can be added to a Microsoft DI `ServiceCollection`.


```
// Template entry point
[DependencyModule]
[CompiledTemplateEngineRuntime.Module]
public partial class TemplateProjectModule { }

// hello-world-template.html
{{model TemplateProject.Models.HelloWorldModel}}
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>{{FirstWord}} {{SecondWord}}</title>
</head>
<body>{{FirstWord}} {{SecondWord}}</body>
</html>

// container configuration
var serviceCollection = new ServiceCollection();
serviceCollection.AddModule(new TemplateProjectModule());
var provider = serviceCollection.BuildServiceProvider();

// Invoke template with type safe invoker
var invoker = provider.GetRequiredService<TemplateProjectModule.Invoker>();
var templateStringOutput =
 await invoker.HelloWorldTemplate(new HelloWorldModel("Hello", "World"));

// Invoke template by name
var templateEngine = provider.GetRequiredService<ITemplateExecutionService>();
var templateStringOutput =
  await templateEngine.Execute("hello-world-template", new HelloWorldModel("Hello", "World"));
```
