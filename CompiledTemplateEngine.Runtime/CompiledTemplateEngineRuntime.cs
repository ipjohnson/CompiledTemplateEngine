using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime;

[DependencyModule]
public partial class CompiledTemplateEngineRuntime {
    /// <summary>
    /// Namespace to be added to all template names and helpers.
    /// </summary>
    public string Namespace { get; set; } = "";
}