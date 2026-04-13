using System;

namespace WB.Logging.LogSinks.Console.System;

internal static class StringExtensions
{
    public static void WriteToConsole(this string @this)
        => global::System.Console.WriteLine(@this);

    public static void WriteToConsole(this string @this, ConsoleColor foregroundColor)
    {
        ConsoleColor originalForegroundColor = global::System.Console.ForegroundColor;
        global::System.Console.ForegroundColor = foregroundColor;
        global::System.Console.WriteLine(@this);
        global::System.Console.ForegroundColor = originalForegroundColor;
    }
}