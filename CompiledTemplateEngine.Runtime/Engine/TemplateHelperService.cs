using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;

[SingletonService(ServiceType = typeof(ITemplateHelperService))]
public class TemplateHelperService : ITemplateHelperService {
    private readonly IEnumerable<ITemplateHelperProvider> _providers;

    public TemplateHelperService(IEnumerable<ITemplateHelperProvider> providers) {
        _providers = providers.Reverse();
    }

    public TemplateHelperFactory LocateHelper(string helperToken) {
        foreach (var helperProvider in _providers) {
            var factor = helperProvider.GetTemplateHelperFactory(helperToken);

            if (factor != null) {
                return factor;
            }
        }

        throw new Exception($"Could not locate token helper for {helperToken}");
    }
}