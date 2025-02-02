using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;

[SingletonService]
public class StringEscapeServiceProvider : IStringEscapeServiceProvider {
    private readonly IEnumerable<IStringEscapeService> _escapeServices;
    private readonly NoopEscapeStringService _noopEscapeStringService = new ();

    public StringEscapeServiceProvider(IEnumerable<IStringEscapeService> escapeServices) {
        _escapeServices = escapeServices.Reverse();
    }

    public IStringEscapeService GetEscapeService(string templateExtension) {
        foreach (var escapeService in _escapeServices) {
            if (escapeService.CanEscapeTemplate(templateExtension)) {
                return escapeService;
            }
        }

        return _noopEscapeStringService;
    }
}