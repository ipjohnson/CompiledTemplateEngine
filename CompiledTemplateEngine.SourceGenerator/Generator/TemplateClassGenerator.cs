using CompiledTemplateEngine.SourceGenerator.Models;
using CSharpAuthor;
using DependencyModules.SourceGenerator.Impl.Models;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class TemplateClassGenerator {
    private CancellationToken _cancellationToken;
    private TemplateImplementationGenerator _implementationGenerator;

    public TemplateClassGenerator(CancellationToken cancellationToken) {
        _cancellationToken = cancellationToken;
        _implementationGenerator = new TemplateImplementationGenerator(_cancellationToken);
    }

    public void Generate(
        CSharpFileDefinition fileDefinition, 
        ModuleEntryPointModel entryModel, 
        TemplateModel templateModel) {
        var templateClass = fileDefinition.AddClass(templateModel.TemplateClassName);

        templateClass.AddBaseType(KnownTemplateTypes.Interfaces.ITemplateExecutionHandler);
        
        ProcessTemplateNodes(templateClass, templateModel.TemplateActionNodes);
        
        GenerateConstructor(templateClass, templateModel);

        _implementationGenerator.GenerateImplementation(templateClass, entryModel, templateModel);
    }

    private static void GenerateConstructor(ClassDefinition templateClass, TemplateModel templateModel) {
        var constructor = templateClass.AddConstructor();

        var services = 
            templateClass.AddField(KnownTemplateTypes.Interfaces.IInternalTemplateServices, "_services");
        
        var servicesParameter = 
            constructor.AddParameter(KnownTemplateTypes.Interfaces.IInternalTemplateServices, "services");
        
        constructor.Assign(servicesParameter).To(services.Instance);
        
        var executionField = 
            templateClass.AddField(
                KnownTemplateTypes.Interfaces.ITemplateExecutionService, "_templateExecutionService");
        
        var executionParameter = 
            constructor.AddParameter(
                KnownTemplateTypes.Interfaces.ITemplateExecutionService, "templateExecutionService");
        
        constructor.Assign(executionParameter).To(executionField.Instance);
        
        var stringEscape = 
            templateClass.AddField(KnownTemplateTypes.Interfaces.IStringEscapeService, "_stringEscapeService");
        
        constructor.Assign(
            servicesParameter.Property("StringEscapeServiceProvider").Invoke(
                "GetEscapeService",
                SyntaxHelpers.QuoteString(templateModel.TemplateExtension))).
            To(stringEscape.Instance);
    }

    private void ProcessTemplateNodes(ClassDefinition classDefinition, IList<TemplateActionNode> templateNodes) {
        foreach (var templateNode in templateNodes) {
            if (templateNode.Action == TemplateActionType.Content) {
                if (!string.IsNullOrEmpty(templateNode.ActionText)) {
                    var initializeString = templateNode.ActionText;

                    initializeString = initializeString.Replace("\"", "\"\"");

                    initializeString = "@\"" + initializeString + "\"";

                    var fieldName = "_contentField" + classDefinition.Fields.Count;

                    var field = classDefinition.AddField(typeof(string), fieldName);

                    field.Modifiers = ComponentModifier.Static | ComponentModifier.Readonly;
                    field.InitializeValue = 
                        CodeOutputComponent.Get(initializeString);

                    templateNode.FieldName = fieldName;
                }
            }
            else {
                ProcessTemplateNodes(classDefinition, templateNode.ArgumentList);
                ProcessTemplateNodes(classDefinition, templateNode.ChildNodes);
            }
        }
    }
}