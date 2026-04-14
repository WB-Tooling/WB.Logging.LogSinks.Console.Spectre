using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressConsoleMessageWriter : IAsyncLogMessageWriter<ProgressPayload, IAnsiConsole>
{
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    public async ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressPayload? payload)
    {
        if (payload is null)
        {
            return;
        }

        await Writer.Progress()
            .AutoClear(payload.AutoClear)
            .AutoRefresh(payload.AutoRefresh)
            .HideCompleted(payload.HideCompleted)
            .StartAsync(async context =>
            {
                try
                {
                    await payload.Progress(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    payload.SetException(ex);
                }
            }).ConfigureAwait(false);

        payload.SetCompleted();
    }
}