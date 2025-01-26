using CompiledTemplateEngine.Runtime.Interfaces;
using CompiledTemplateEngine.Runtime.Utilities;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;

[SingletonService(ServiceType = typeof(IInternalTemplateServices))]
public class InternalTemplateServices : IInternalTemplateServices {
    public InternalTemplateServices(
        IStringBuilderPool stringBuilderPool,
        IDataFormattingService dataFormattingService,
        ITemplateHelperService templateHelperService,
        IBooleanLogicService booleanLogicService,
        IStringEscapeServiceProvider stringEscapeServiceProvider) {
        StringBuilderPool = stringBuilderPool;
        DataFormattingService = dataFormattingService;
        TemplateHelperService = templateHelperService;
        BooleanLogicService = booleanLogicService;
        StringEscapeServiceProvider = stringEscapeServiceProvider;
    }

    public IStringBuilderPool StringBuilderPool { get; }

    public IDataFormattingService DataFormattingService { get; }

    public ITemplateHelperService TemplateHelperService { get; }

    public IBooleanLogicService BooleanLogicService { get; }

    public IStringEscapeServiceProvider StringEscapeServiceProvider { get; }
}