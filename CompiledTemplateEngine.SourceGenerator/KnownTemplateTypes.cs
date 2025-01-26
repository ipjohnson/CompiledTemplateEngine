using CSharpAuthor;
// ReSharper disable InconsistentNaming

using System.Collections.Immutable;

namespace CompiledTemplateEngine.SourceGenerator;

public class KnownTemplateTypes {
    public static class System {
        public static class ImmutableCollections {
            public static readonly ITypeDefinition Dictionary = TypeDefinition.Get(typeof(ImmutableDictionary<,>));
        }
    }
    
    public static class Attributes {
        
        public const string Namespace = "CompiledTemplateEngine.Runtime.Attributes";
        
        public static readonly ITypeDefinition TemplateHelperAttribute = 
            TypeDefinition.Get(Namespace, "TemplateHelperAttribute");
    }
    
    public static class Engine {
        public const string Namespace = "CompiledTemplateEngine.Runtime.Engine";
        
        public static ITypeDefinition TemplateExecutionContext { get; } =
            TypeDefinition.Get(Namespace, "TemplateExecutionContext");
    }
    
    public static class Interfaces {
        public const string Namespace = "CompiledTemplateEngine.Runtime.Interfaces";

        public static readonly ITypeDefinition ITemplateHelper =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace, "ITemplateHelper");
        
        
        public static ITypeDefinition ITemplateExecutionService { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateExecutionService");

        public static ITypeDefinition ITemplateOutputWriter { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateOutputWriter");

        public static ITypeDefinition ITemplateExecutionHandler { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateExecutionHandler");

        public static ITypeDefinition ITemplateExecutionContext { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateExecutionContext");

        public static ITypeDefinition IInternalTemplateServices { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "IInternalTemplateServices");

        public static ITypeDefinition ITemplateExecutionHandlerProvider { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateExecutionHandlerProvider");

        public static ITypeDefinition IStringEscapeService { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "IStringEscapeService");

        public static ITypeDefinition DefaultOutputFuncHelper { get; } =
            TypeDefinition.Get(Namespace, "DefaultOutputFuncHelper");

        public static ITypeDefinition ITemplateHelperProvider { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateHelperProvider");


        public static ITypeDefinition TemplateHelperFactory { get; } =
            TypeDefinition.Get(Namespace, "TemplateHelperFactory");


        public static ITypeDefinition TemplateExecutionService { get; } =
            TypeDefinition.Get(TypeDefinitionEnum.InterfaceDefinition, Namespace,
                "ITemplateExecutionService");

        public static ITypeDefinition TemplateExecutionFunction { get; } =
            TypeDefinition.Get(Namespace, "TemplateExecutionFunction");

        public static ITypeDefinition TemplateHelperAttribute { get; } =
            TypeDefinition.Get(Namespace, "TemplateHelperAttribute");

        public static ITypeDefinition DefaultHelpers { get; } =
            TypeDefinition.Get("Hardened.Templates.Runtime.Helpers", "DefaultHelpers");
    
    }
}