namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface ITemplateHelperProvider {
    TemplateHelperFactory? GetTemplateHelperFactory(string mustacheToken);
}