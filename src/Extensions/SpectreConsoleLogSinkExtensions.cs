using System;

namespace WB.Logging.LogSinks.Console.Spectre;

internal static class SpectreConsoleLogSinkExtensions
{
    internal static IDisposable Disable(this SpectreConsoleLogSink logSink)
        => logSink.AddFilter(_ => false);
}

