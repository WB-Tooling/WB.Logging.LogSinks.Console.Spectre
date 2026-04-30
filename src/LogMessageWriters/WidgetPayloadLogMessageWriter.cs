using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class PayloadLogMessageWriter : IAsyncLogMessageWriter<SpectreConsoleLogSink, WidgetPayload>
{
    [NotNull]
    public SpectreConsoleLogSink? LogSink { get; set; }

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, WidgetPayload? payload)
    {
        if (payload is not null)
        {
            LogSink?.Console.Write(payload.Widget);
        }

        return ValueTask.CompletedTask;
    }
}
