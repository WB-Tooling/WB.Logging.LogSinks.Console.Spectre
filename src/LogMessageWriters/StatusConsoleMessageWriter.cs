using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's status to 
/// render status updates in the console.
/// </summary>
internal sealed class StatusConsoleMessageWriter
    : IAsyncLogMessageWriter<SpectreConsoleLogSink, StatusStartPayload>
    , IAsyncLogMessageWriter<SpectreConsoleLogSink, StatusFinishedPayload>
{
    private IDisposable? logSinkDisabledSubscription;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    /// <inheritdoc/>
    [NotNull]
    public SpectreConsoleLogSink? LogSink { get; set; }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, StatusStartPayload payload)
    {
        if (logSinkDisabledSubscription is null && LogSink is not null)
        {
            logSinkDisabledSubscription = LogSink.AddFilter<object>(lm => lm.Payload is StatusFinishedPayload);

            payload.SetStatus(LogSink.Console.Status());
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, StatusFinishedPayload payload)
    {
        if (logSinkDisabledSubscription is not null)
        {
            logSinkDisabledSubscription.Dispose();
            logSinkDisabledSubscription = null;

            payload.SetFinished();
        }

        return ValueTask.CompletedTask;
    }
}
