using System.Collections.Immutable;
using DependencyModules.SourceGenerator.Impl.Models;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class EntryModelSelector {

    public static bool ModelIsAttributed(ModuleEntryPointModel entryPointModel) {
        return entryPointModel.AttributeModels.Any(
            o => o.TypeDefinition.Name.StartsWith("CompiledTemplateEngineRuntime"));
    }

    public static (ModuleEntryPointModel entryModel, DependencyModuleConfigurationModel configurationModel)? Select(ImmutableArray<(ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)> entryModelAndConfig) {
        foreach (var tuple in entryModelAndConfig) {
            if (ModelIsAttributed(tuple.Left)) {
                return tuple;
            }
        }
        
        return null;
    }
}