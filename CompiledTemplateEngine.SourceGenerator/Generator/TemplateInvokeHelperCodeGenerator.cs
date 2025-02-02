﻿using System.Text;
using static CSharpAuthor.SyntaxHelpers;
using CompiledTemplateEngine.SourceGenerator.Models;

namespace CompiledTemplateEngine.SourceGenerator.Generator;

internal class TemplateInvokeHelperCodeGenerator {
    public void WriteTemplateHelperToBuilder(TemplateImplementationGenerator.GenerationContext context) {
        var variableName = AssignNodeToVariable(context);

        var writeMethod =
            context.CurrentNode!.Action == TemplateActionType.RawMustacheToken ? "WriteRaw" : "Write";

        context.CurrentBlock.AddCode(
            $"writer.{writeMethod}(_services.DataFormattingService.FormatData(executionContext, \"{context.CurrentNode!.ActionText}\", {variableName}));");
    }

    public string AssignNodeToVariable(TemplateImplementationGenerator.GenerationContext context) {
        var argumentList = ProcessNodeArguments(context);

        var functionName = InitializeHelper(context);
        var variableName = context.InvokeMethod.GetUniqueVariable("helperOutput");

        context.CurrentBlock.Assign(Await($"{functionName}(serviceProvider).Execute(executionContext{argumentList})"))
            .ToVar(variableName);

        return variableName;
    }

    private string InitializeHelper(TemplateImplementationGenerator.GenerationContext context) {
        var helperName = context.CurrentNode!.ActionText.Substring(1);

        var fieldName = "_helper_" + GetSafeName(helperName);

        if (context.ClassDefinition.Fields.Any(f => f.Name == fieldName)) {
            return fieldName;
        }

        context.ClassDefinition.AddField(KnownTemplateTypes.Interfaces.TemplateHelperFactory, fieldName);

        return fieldName;
    }

    private string GetSafeName(string helperName) {
        return helperName.Replace('-', '_').Replace('.', '_');
    }

    private string ProcessNodeArguments(TemplateImplementationGenerator.GenerationContext context) {
        var returnString = new StringBuilder();

        foreach (var argumentNode in context.CurrentNode!.ArgumentList) {
            if (argumentNode.Action == TemplateActionType.StringLiteral) {
                returnString.Append(", \"");
                returnString.Append(argumentNode.ActionText);
                returnString.Append("\"");
            }
            else if (argumentNode.Action == TemplateActionType.MustacheToken) {
                if (argumentNode.ActionText.StartsWith("$")) {
                    var currentNode = context.CurrentNode;

                    var variableName = AssignNodeToVariable(context);

                    returnString.Append(", ");
                    returnString.Append(variableName);

                    context.CurrentNode = currentNode;
                }
                else if (argumentNode.ActionText.StartsWith("`")) {
                    var propertyName = argumentNode.ActionText.Substring(1);
                    returnString.Append(", ");
                    returnString.Append("PropertyValue.From(");
                    returnString.Append(context.CurrentModel.Name + "." + propertyName);
                    returnString.Append(", \"" + propertyName + "\")");
                }
                else if (argumentNode.ActionText == ".") {
                    returnString.Append(", ");
                    returnString.Append(context.CurrentModel.Name);
                }
                else {
                    returnString.Append(", ");
                    returnString.Append(context.CurrentModel.Name + "." + argumentNode.ActionText);
                }
            }
        }

        return returnString.ToString();
    }
}