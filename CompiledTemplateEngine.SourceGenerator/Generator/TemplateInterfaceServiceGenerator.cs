using System.Collections.Immutable;
using CompiledTemplateEngine.SourceGenerator.Models;
using CSharpAuthor;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class TemplateInterfaceServiceGenerator {
    private CancellationToken _cancellationToken;

    public TemplateInterfaceServiceGenerator(CancellationToken cancellationToken) {
        _cancellationToken = cancellationToken;
    }

    public void GenerateInterfaceService(SourceProductionContext context,
        ModuleEntryPointModel moduleEntryPoint, 
        DependencyModuleConfigurationModel configurationModel,
        ImmutableArray<TemplateModel> templateModels) {

        var csharpFile = new CSharpFileDefinition(moduleEntryPoint.EntryPointType.Namespace);

        var classDefinition = GetClassDefinition(moduleEntryPoint, csharpFile);
        
        GenerateInvokeInterface(moduleEntryPoint, templateModels, classDefinition);
        GenerateTemplateProvider( moduleEntryPoint, templateModels, classDefinition);
        
        var outputContext = new OutputContext();
        
        csharpFile.WriteOutput(outputContext);
        
        context.AddSource($"{moduleEntryPoint.EntryPointType.Name}.Interface.g.cs", outputContext.Output());
    }
    private void GenerateTemplateProvider(ModuleEntryPointModel moduleEntryPoint,
        ImmutableArray<TemplateModel> templateModels, ClassDefinition classDefinition) {
        
        var providerClass = classDefinition.AddClass("TemplateProvider");

        providerClass.Modifiers |= ComponentModifier.Private;
        providerClass.AddBaseType(KnownTemplateTypes.Interfaces.ITemplateExecutionHandlerProvider);

        providerClass.AddProperty(KnownTemplateTypes.Interfaces.ITemplateExecutionService, "TemplateExecutionService");
        var internalService = providerClass.AddField(KnownTemplateTypes.Interfaces.IInternalTemplateServices, "_internalServices");

        var constructor = providerClass.AddConstructor();
        
        var parameter = constructor.AddParameter(KnownTemplateTypes.Interfaces.IInternalTemplateServices, "internalServices");
        constructor.Assign(parameter).To(internalService.Instance);
        
        GenerateTemplateProviderMethod(providerClass, moduleEntryPoint, templateModels);
    }

    private void GenerateTemplateProviderMethod(ClassDefinition providerClass, ModuleEntryPointModel moduleEntryPoint, ImmutableArray<TemplateModel> templateModels) {
        var method = providerClass.AddMethod("GetTemplateExecutionHandler");
        
        method.SetReturnType(KnownTemplateTypes.Interfaces.ITemplateExecutionHandler);
        var templateName = method.AddParameter(typeof(string), "templateName");

        var switchBlock = method.Switch(templateName);

        var templateClassNamespace = moduleEntryPoint.EntryPointType.Namespace + "." + "Generated.Templates";
        
        foreach (var templateModel in templateModels) {
            var type = TypeDefinition.Get(templateClassNamespace, templateModel.TemplateClassName);
            
            var templateField = providerClass.AddField(type, "templateField" + providerClass.Fields.Count);

            var newCase = switchBlock.AddCase(SyntaxHelpers.QuoteString(templateModel.TemplateName));

            var newStatement = SyntaxHelpers.New(type, "_internalServices", providerClass.Properties[0].Instance);

            var nullCoalesceEqual = 
                SyntaxHelpers.NullCoalesceEqual(templateField.Instance, newStatement);
            
            newCase.Return(nullCoalesceEqual);
        }
        
        switchBlock.AddDefault().Return(SyntaxHelpers.Null());
    }

    private void GenerateInvokeInterface(ModuleEntryPointModel moduleEntryPoint, ImmutableArray<TemplateModel> templateModels,ClassDefinition classDefinition ) {
        
        GenerateInterface(moduleEntryPoint,templateModels, classDefinition);
        
        GenerateImplementation(moduleEntryPoint, templateModels, classDefinition);
    }

    private void GenerateImplementation(
        ModuleEntryPointModel moduleEntryPoint, 
        ImmutableArray<TemplateModel> templateModels, 
        ClassDefinition classDefinition) {
        var implementation = classDefinition.AddClass("InvokerImplementation");

        implementation.Modifiers |= ComponentModifier.Private;
        implementation.AddBaseType(TypeDefinition.Get("", "IInvoker"));
        
        var executionService = AddImplementationConstructor(implementation);
        
        ImplementMethods(implementation, executionService, templateModels);
    }

    private void ImplementMethods(ClassDefinition implementation, FieldDefinition executionService, ImmutableArray<TemplateModel> templateModels) {
        foreach (var templateModel in templateModels) {
            ImplementStringMethod(implementation, executionService, templateModel);
            
            ImplementOutputMethod(implementation, executionService, templateModel);
        }
    }

    private void ImplementOutputMethod(ClassDefinition implementation, FieldDefinition executionService, TemplateModel templateModel) {
        var invokeMethod = implementation.AddMethod(templateModel.SafeName);
        var modelNameAndSpace = GetModelNameAndSpaces(templateModel.TemplateActionNodes);

        foreach (var namespaceStr in modelNameAndSpace.Namespaces) {
            invokeMethod.AddUsingNamespace(namespaceStr);
        }
        
        ParameterDefinition? modelParameter = null;
        
        if (modelNameAndSpace.ModelName != null) {
            modelParameter = 
                invokeMethod.AddParameter(TypeDefinition.Get("", modelNameAndSpace.ModelName), "model");
        }

        var writerParameter = 
            invokeMethod.AddParameter(KnownTemplateTypes.Interfaces.ITemplateOutputWriter, "writer");
        
        var customDataParameter = 
            invokeMethod.AddParameter(typeof(ImmutableDictionary<string,object>), "customData");
        
        var serviceProviderParameter = 
            invokeMethod.AddParameter(typeof(IServiceProvider), "serviceProvider");
        
        invokeMethod.SetReturnType(typeof(Task));
        
        var allParameters = new List<object> {
            writerParameter,
            customDataParameter,
            serviceProviderParameter
        };
        
        allParameters.Insert(0, modelParameter ?? SyntaxHelpers.Null());
        allParameters.Insert(0, SyntaxHelpers.QuoteString(templateModel.TemplateName));
        
        invokeMethod.Return(
            executionService.Instance.Invoke("Execute", allParameters.OfType<object>().ToArray()));
    }

    private void ImplementStringMethod(ClassDefinition implementation, FieldDefinition executionService, TemplateModel templateModel) {
        var invokeMethod = implementation.AddMethod(templateModel.SafeName);
        var modelNameAndSpace = GetModelNameAndSpaces(templateModel.TemplateActionNodes);

        foreach (var namespaceStr in modelNameAndSpace.Namespaces) {
            invokeMethod.AddUsingNamespace(namespaceStr);
        }
        
        ParameterDefinition? modelParameter = null;
        
        if (modelNameAndSpace.ModelName != null) {
            modelParameter = 
                invokeMethod.AddParameter(TypeDefinition.Get("", modelNameAndSpace.ModelName), "model");
        }

        var customDataParameter = 
            invokeMethod.AddParameter(typeof(ImmutableDictionary<string,object>), "customData");
        
        var serviceProviderParameter = 
            invokeMethod.AddParameter(typeof(IServiceProvider), "serviceProvider");
        
        invokeMethod.SetReturnType(typeof(Task<string>));
        
        var allParameters = new List<object> {
            customDataParameter,
            serviceProviderParameter
        };
        
        allParameters.Insert(0, modelParameter ?? SyntaxHelpers.Null());
        allParameters.Insert(0, SyntaxHelpers.QuoteString( templateModel.TemplateName));
        
        invokeMethod.Return(
            executionService.Instance.Invoke("Execute", allParameters.OfType<object>().ToArray()));
    }

    private FieldDefinition AddImplementationConstructor(ClassDefinition implementation) {
        var executionService = 
            implementation.AddField(KnownTemplateTypes.Interfaces.ITemplateExecutionService, "_executionService");

        var implementationConstructor = implementation.AddConstructor();
        
        var parameter = implementationConstructor.AddParameter(KnownTemplateTypes.Interfaces.ITemplateExecutionService, "executionService");
        
        implementationConstructor.Assign(parameter).To(executionService.Instance);
        
        return executionService;
    }

    private void GenerateInterface(ModuleEntryPointModel moduleEntryPoint, ImmutableArray<TemplateModel> templateModels, ClassDefinition classDefinition) {
        var interfaceDefinition = new InterfaceDefinition("IInvoker");

        foreach (var templateModel in templateModels) {
            _cancellationToken.ThrowIfCancellationRequested();
            
            GenerateStringMethodForTemplate(moduleEntryPoint, templateModel, interfaceDefinition);
            
            GenerateOutputWriterMethodForTemplate(moduleEntryPoint, templateModel, interfaceDefinition);
        }
        
        classDefinition.AddComponent(CodeOutputComponent.Get("#nullable enable"));
        classDefinition.AddComponent(interfaceDefinition);
        classDefinition.AddComponent(CodeOutputComponent.Get("#nullable disable\n"));
    }

    private void GenerateOutputWriterMethodForTemplate(ModuleEntryPointModel moduleEntryPoint, TemplateModel templateModel, InterfaceDefinition interfaceDefinition) {
        var invokeMethod = interfaceDefinition.AddMethod(templateModel.SafeName);
        var modelNameAndSpace = GetModelNameAndSpaces(templateModel.TemplateActionNodes);

        foreach (var namespaceStr in modelNameAndSpace.Namespaces) {
            invokeMethod.AddUsingNamespace(namespaceStr);
        }
        
        if (modelNameAndSpace.ModelName != null) {
            invokeMethod.AddParameter(TypeDefinition.Get("", modelNameAndSpace.ModelName), "data");
        }

        invokeMethod.AddParameter(KnownTemplateTypes.Interfaces.ITemplateOutputWriter, "writer");
        
        var customData = 
            invokeMethod.AddParameter(TypeDefinition.Get(typeof(ImmutableDictionary<string,object>)).MakeNullable(), "customData");

        customData.DefaultValue = SyntaxHelpers.Null();
        
        var serviceProvider = 
            invokeMethod.AddParameter(TypeDefinition.Get(typeof(IServiceProvider)).MakeNullable(), "serviceProvider");
        
        serviceProvider.DefaultValue = SyntaxHelpers.Null();
        
        invokeMethod.SetReturnType(typeof(Task));
    }

    private void GenerateStringMethodForTemplate(ModuleEntryPointModel moduleEntryPoint, TemplateModel templateModel, InterfaceDefinition interfaceDefinition) {
        var invokeMethod = interfaceDefinition.AddMethod(templateModel.SafeName);
        var modelNameAndSpace = GetModelNameAndSpaces(templateModel.TemplateActionNodes);

        foreach (var namespaceStr in modelNameAndSpace.Namespaces) {
            invokeMethod.AddUsingNamespace(namespaceStr);
        }
        
        if (modelNameAndSpace.ModelName != null) {
            invokeMethod.AddParameter(CSharpAuthor.TypeDefinition.Get("", modelNameAndSpace.ModelName), "data");
        }

        var customData = 
            invokeMethod.AddParameter(TypeDefinition.Get(typeof(ImmutableDictionary<string,object>)).MakeNullable(), "customData");

        customData.DefaultValue = SyntaxHelpers.Null();
        
        var serviceProvider = 
            invokeMethod.AddParameter(TypeDefinition.Get(typeof(IServiceProvider)).MakeNullable(), "serviceProvider");
        
        serviceProvider.DefaultValue = SyntaxHelpers.Null();
        
        invokeMethod.SetReturnType(typeof(Task<string>));
    }

    private (string? ModelName, List<string> Namespaces) GetModelNameAndSpaces(IList<TemplateActionNode> templateModelTemplateActionNodes) {
        var namespaces = new List<string>();

        foreach (var templateNode in templateModelTemplateActionNodes) {
            if (templateNode.ActionText == "using" && templateNode.ArgumentList.Count > 0) {
                namespaces.Add(templateNode.ArgumentList[0].ActionText);
            }
            else if (templateNode is { ActionText: "model", ArgumentList.Count: > 0 }) {
                return (templateNode.ArgumentList[0].ActionText, namespaces);
            }
        }

        return (null, namespaces);
    }

    private ClassDefinition GetClassDefinition(ModuleEntryPointModel moduleEntryPoint, CSharpFileDefinition csharpFile) {
        var classDefinition = csharpFile.AddClass(moduleEntryPoint.EntryPointType.Name);

        classDefinition.Modifiers |= ComponentModifier.Partial;
        
        return classDefinition;
    }
}