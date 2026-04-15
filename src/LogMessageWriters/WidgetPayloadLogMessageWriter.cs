using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class PayloadLogMessageWriter : IAsyncLogMessageWriter<WidgetPayload, IAnsiConsole>
{
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    public IAsyncLogSink? LogSink { get; set; }

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, WidgetPayload? payload)
    {
        if (payload is not null)
        {
            Writer.Write(payload.Widget);
        }

        return ValueTask.CompletedTask;
    }
}
