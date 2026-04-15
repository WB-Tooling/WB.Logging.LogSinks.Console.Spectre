using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Json;
using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

internal static class ObjectExtensions
{
    private static readonly ConcurrentDictionary<Type, bool> ToStringOverrideCache = new();

    internal static bool HasOverriddenToString<TObject>(this TObject @this)
    {
        if (@this is null)
        {
            return false;
        }

        Type type = @this.GetType();

        return ToStringOverrideCache.GetOrAdd(type, type =>
        {
            MethodInfo? methodInfo = type.GetMethod(nameof(ToString), Type.EmptyTypes);

            return methodInfo?.DeclaringType != typeof(object);
        });
    }

    internal static IRenderable ToRenderable<TObject>(this TObject @this, Style? style = null)
    {
        if (@this is null)
        {
            return new Text("null", style);
        }

        if (@this is IRenderable renderable)
        {
            return renderable;
        }

        if (@this is string @string)
        {
            return @string.ToMarkup(style);
        }

        if (@this.HasOverriddenToString())
        {
            return new Text(@this.ToString() ?? string.Empty, style);
        }
        else
        {
            return new JsonText(@this.ToJsonString());
        }
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Methods                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static string ToJsonString(this object @this)
        => JsonSerializer.Serialize(@this);
}
