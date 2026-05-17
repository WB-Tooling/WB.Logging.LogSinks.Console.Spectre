using System;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal static class StringExtensions
{
    public static Markup ToMarkup(this string? @this, Style? style = null)
    {
        try
        {
            return new Markup(@this ?? string.Empty, style);
        }
        catch (InvalidOperationException)
        {
            return new Markup(@this?.EscapeMarkup() ?? string.Empty, style);
        }
    }
}
