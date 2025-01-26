using CompiledTemplateEngine.Runtime.Utilities;

namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface IInternalTemplateServices {
    IStringBuilderPool StringBuilderPool { get; }
    
    IDataFormattingService DataFormattingService { get; }

    ITemplateHelperService TemplateHelperService { get; }

    IBooleanLogicService BooleanLogicService { get; }

    IStringEscapeServiceProvider StringEscapeServiceProvider { get; }
}