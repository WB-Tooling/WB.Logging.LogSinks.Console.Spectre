using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's status to 
/// render status updates in the console.
/// </summary>
internal sealed class StatusConsoleMessageWriter
    : IAsyncLogMessageWriter<StatusStartPayload, IAnsiConsole>
    , IAsyncLogMessageWriter<StatusFinishedPayload, IAnsiConsole>
{
    private readonly StatusFinishedPayloadOnlyFilter statusFinishedPayloadOnlyFilter = new();

    private IDisposable? logSinkDisabledSubscription;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    /// <inheritdoc/>
    public IAsyncLogSink? LogSink { get; set; }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, StatusStartPayload payload)
    {
        if (logSinkDisabledSubscription is null)
        {
            logSinkDisabledSubscription = LogSink?.AddFilter(statusFinishedPayloadOnlyFilter);

            payload.SetStatus(Writer.Status());
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
