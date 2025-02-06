using CSharpAuthor;
using DependencyModules.SourceGenerator.Impl;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class TemplateDependencyInjectionGenerator {

    public void Generate(SourceProductionContext context,
        (ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)  data) {
        var model = data.Left;
        
        if (!EntryModelSelector.ModelIsAttributed(model)) {
            return;
        }

        var fileWriter = new DependencyFileWriter();

        var output =
            fileWriter.Write(model, new DependencyModuleConfigurationModel(RegistrationType.Add), GetServiceModels(), "TemplateDeps");

        context.AddSource($"{model.EntryPointType.Name}.TemplateDeps.g.cs", output);
    }

    private IEnumerable<ServiceModel> GetServiceModels() {
        yield return new ServiceModel(TypeDefinition.Get("", "InvokerImplementation"),
            new[] {
                new ServiceRegistrationModel(
                    TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, "", "IInvoker"),
                    ServiceLifestyle.Singleton,
                    null, null, null
                )
            });
        yield return new ServiceModel(TypeDefinition.Get("", "TemplateProvider"),
            new[] {
                new ServiceRegistrationModel(
                    KnownTemplateTypes.Interfaces.ITemplateExecutionHandlerProvider,
                    ServiceLifestyle.Singleton,
                    null, null, null
                )
            });
    }
}