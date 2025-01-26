using System.Collections.Immutable;
using CompiledTemplateEngine.Runtime.Engine;
using CompiledTemplateEngine.Runtime.Helpers;
using CompiledTemplateEngine.Runtime.Interfaces;
using CompiledTemplateEngine.Runtime.Utilities;
using NSubstitute;
using Xunit;

namespace CompiledTemplateEngine.Runtime.Tests.Helpers;

public abstract class BaseHelperTests {
    protected abstract Type TemplateHelperType { get; }

    protected abstract string Token { get; }

    [Fact]
    public void FindDefaultHelper() {
        var helper = GetHelper();

        Assert.Equal(TemplateHelperType, helper.GetType());
    }

    [Fact]
    public void DefaultHelperIsSingleton() {
        var defaultHelper = new DefaultHelpers();

        var helperFactory = defaultHelper.GetTemplateHelperFactory(Token);

        Assert.NotNull(helperFactory);

        var helper = helperFactory(null!);
        Assert.Same(helper, helperFactory(null!));

        var helperFactory2 = defaultHelper.GetTemplateHelperFactory(Token);

        if (helperFactory2 == null) {
            throw new Exception("Default helper factory returned null for helperFactory2 call");
        }
        
        Assert.Same(helperFactory, helperFactory2);
        Assert.Same(helper, helperFactory2(null!));
    }

    protected ITemplateHelper GetHelper() {
        var defaultHelper = new DefaultHelpers();

        var helperFactory = defaultHelper.GetTemplateHelperFactory(Token);

        Assert.NotNull(helperFactory);

        var helper = helperFactory(null!);

        Assert.NotNull(helper);

        return helper;
    }


    protected ITemplateExecutionContext GetExecutionContext(object? data = null) {
        var htmlEscapeStringService = new HtmlEscapeStringService();

        var stringBuilderPool = new StringBuilderPool();

        var internalServices = new InternalTemplateServices(
            stringBuilderPool,
            new DataFormattingService(Array.Empty<IDataFormatProvider>()),
            new TemplateHelperService(Array.Empty<ITemplateHelperProvider>()),
            new BooleanLogicService(),
            new StringEscapeServiceProvider(new[] { htmlEscapeStringService })
        );

        var builder = stringBuilderPool.Get().Item;

        var writer = new StringBuilderTemplateOutputWriter(builder);

        return new TemplateExecutionContext(
            "html",
            Substitute.For<IServiceProvider>(),
            data ?? new object(),
            internalServices,
            Substitute.For<ITemplateExecutionService>(),
            htmlEscapeStringService,
            writer,
            ImmutableDictionary<string, object>.Empty,
        null
        );
    }
}