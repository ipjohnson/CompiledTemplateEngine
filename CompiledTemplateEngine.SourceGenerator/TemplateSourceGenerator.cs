using CompiledTemplateEngine.SourceGenerator.Generator;
using DependencyModules.SourceGenerator.Impl;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;
using ISourceGenerator = DependencyModules.SourceGenerator.Impl.ISourceGenerator;

namespace CompiledTemplateEngine.SourceGenerator;

[Generator]
public class TemplateSourceGenerator : BaseSourceGenerator {
    
    protected override IEnumerable<ISourceGenerator> AttributeSourceGenerators() {
        yield return new TemplateHelperSourceGenerator();
        yield return new TemplateModuleSourceGenerator();
    }

    protected override void SetupRootGenerator(
        IncrementalGeneratorInitializationContext context, 
        IncrementalValuesProvider<(ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)> valuesProvider) {
            var generator = new TemplateDependencyInjectionGenerator();
        
            context.RegisterSourceOutput(valuesProvider, generator.Generate);
    }
}