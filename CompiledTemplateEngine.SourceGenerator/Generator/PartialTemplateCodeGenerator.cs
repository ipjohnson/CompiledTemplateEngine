using CSharpAuthor;
using static CSharpAuthor.SyntaxHelpers;
namespace CompiledTemplateEngine.SourceGenerator.Generator;

public class PartialTemplateCodeGenerator {
    private readonly TemplateInvokeHelperCodeGenerator _helperCodeGenerator = new();

    public void ProcessPartialTemplateNode(TemplateImplementationGenerator.GenerationContext context) {
        if (context.CurrentNode!.ArgumentList.Count is > 0 and <= 2) {
            var partialTemplateFunction = GetOrCreatePartialTemplateFunctionField(
                context, context.CurrentNode.ArgumentList[0].ActionText);

            var argument = context.CurrentModel.Name;

            if (context.CurrentNode.ArgumentList.Count == 2) {
                var currentNode = context.CurrentNode;

                context.CurrentNode = currentNode.ArgumentList[1];

                argument = GetTemplateArgument(context);

                context.CurrentNode = currentNode;
            }

            var invokeStatement =
                partialTemplateFunction.Instance.Invoke("Invoke",
                    argument,
                    "customData",
                    "serviceProvider",
                    "writer",
                    "executionContext"
                    );

            context.CurrentBlock.AddIndentedStatement(Await(invokeStatement));
        }
    }

    private string GetTemplateArgument(TemplateImplementationGenerator.GenerationContext context) {
        if (context.CurrentNode == null) {
            throw new ArgumentNullException(nameof(context.CurrentNode));
        }
        
        if (context.CurrentNode.ActionText.StartsWith("$")) {
            return _helperCodeGenerator.AssignNodeToVariable(context);
        }

        if (context.CurrentNode.ActionText.EndsWith("/")) {
            return context.CurrentNode.ActionText.Substring(1);
        }

        return context.CurrentModel.Name + "." + context.CurrentNode.ActionText;
    }

    private FieldDefinition GetOrCreatePartialTemplateFunctionField(TemplateImplementationGenerator.GenerationContext context,
        string actionText) {
        var functionFieldName = "_template_" + actionText.Replace('.', '_').Replace('-', '_');
        var templateFunction = 
            context.ClassDefinition.Fields.FirstOrDefault(f => f.Name == functionFieldName);
        
        if (templateFunction == null) {
            templateFunction =
                context.ClassDefinition.AddField(KnownTemplateTypes.Interfaces.TemplateExecutionFunction, functionFieldName);
            
            var templateService = context.ClassDefinition.Fields.First(p => p.TypeDefinition.Equals(
                KnownTemplateTypes.Interfaces.ITemplateExecutionService
            ));
            
            var constructor = context.ClassDefinition.Constructors.First();
            var throwException = new ThrowNewExceptionStatement(
                TypeDefinition.Get(typeof(Exception)),
                new object[] {
                    QuoteString("Could not find ")
                });
            
            var invoke = templateService.Instance.Invoke("FindTemplateExecutionFunction", QuoteString(actionText));
            
            constructor.Assign(NullCoalesce(invoke, throwException)).To(templateFunction.Instance);
        }

        return templateFunction;
    }
}