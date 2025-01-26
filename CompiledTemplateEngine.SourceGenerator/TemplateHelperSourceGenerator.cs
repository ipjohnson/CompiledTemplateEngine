using System.Collections.Immutable;
using CompiledTemplateEngine.SourceGenerator.Models;
using CSharpAuthor;
using DependencyModules.SourceGenerator.Impl;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;

namespace CompiledTemplateEngine.SourceGenerator;

public class TemplateHelperSourceGenerator : BaseAttributeSourceGenerator<TemplateHelperModel> {

    protected override IEnumerable<ITypeDefinition> AttributeTypes() {
        yield return KnownTemplateTypes.Attributes.TemplateHelperAttribute;
    }

    protected override void GenerateSourceOutput(SourceProductionContext context,
        ((ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right) Left, ImmutableArray<TemplateHelperModel> Right) dataModel) {
        
    }

    protected override IEqualityComparer<TemplateHelperModel> GetComparer() {
        return new TemplateHelperComparer();
    }

    protected override TemplateHelperModel GenerateAttributeModel(GeneratorSyntaxContext arg1, CancellationToken arg2) {
        return new TemplateHelperModel();
    }
}