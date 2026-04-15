using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressConsoleMessageWriter : IAsyncLogMessageWriter<ProgressPayload, IAnsiConsole>
{
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    public IAsyncLogSink? LogSink { get; set; }

    public async ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressPayload payload)
    {
        try
        {
            await Writer.Progress()
                .AutoClear(payload.AutoClear)
                .AutoRefresh(payload.AutoRefresh)
                .HideCompleted(payload.HideCompleted)
                .StartAsync(payload.Progress).ConfigureAwait(false);

            payload.SetCompleted();
        }
        catch (Exception exception)
        {
            payload.SetException(exception);
        }
    }
}