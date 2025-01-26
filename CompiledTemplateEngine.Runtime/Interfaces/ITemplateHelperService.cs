namespace CompiledTemplateEngine.Runtime.Interfaces;

public delegate ITemplateHelper TemplateHelperFactory(IServiceProvider serviceProvider);

public interface ITemplateHelperService {
    TemplateHelperFactory LocateHelper(string helperToken);
}