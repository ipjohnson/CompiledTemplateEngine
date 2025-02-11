﻿using CompiledTemplateEngine.Runtime.Interfaces;

namespace CompiledTemplateEngine.Runtime.Helpers.String;

public class ToHelper : ITemplateHelper {
    public ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments) {
        if (arguments.Length == 0) {
            return new ValueTask<object?>("");
        }

        if (arguments.Length >= 2 &&
            arguments[0] is IFormattable formattable &&
            arguments[1] is string formatString) {
            return new ValueTask<object?>(formattable.ToString(formatString, null));
        }

        return new ValueTask<object?>(arguments[0]?.ToString() ?? "");
    }
}