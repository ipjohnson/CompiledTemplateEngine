using System.Collections.Immutable;
using CompiledTemplateEngine.SourceGenerator.Models;
using CSharpAuthor;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class TemplateFileGenerator {

    public void GenerateFile(SourceProductionContext context, 
        TemplateModel templateModel, 
        ImmutableArray<(ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)> entryModelAndConfig) {
        var configData = EntryModelSelector.Select(entryModelAndConfig);

        if (configData is null) {
            return;
        }

        var (fileName, fileContent) = GenerateCode(templateModel, 
            configData.Value.entryModel, configData.Value.configurationModel, context.CancellationToken);

        context.AddSource(fileName, fileContent);
    }

    private (string fileName, string fileContent) GenerateCode(
        TemplateModel templateModel, 
        ModuleEntryPointModel entryModel, 
        DependencyModuleConfigurationModel configuration,
        CancellationToken cancellationToken) {
        
        var fileDefinition = new CSharpFileDefinition(
            entryModel.EntryPointType.Namespace + "." + "Generated.Templates"
            );

        var context = new OutputContext();

        var classGenerator = new TemplateClassGenerator(cancellationToken);

        classGenerator.Generate(fileDefinition, entryModel, templateModel);

        fileDefinition.WriteOutput(context);
        
        return (templateModel.TemplateClassName + ".g.cs", context.Output());
    }

}