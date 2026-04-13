using System;

namespace WB.Logging.LogSinks.Console.System;

internal static class LogLevelExtensions
{
    public static string ToString(this LogLevel @this)
        => @this switch
        {
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERRO",
            _ => "UNKN",
        };
}