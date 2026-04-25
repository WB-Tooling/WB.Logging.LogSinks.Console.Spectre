using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's progress bar to 
/// render progress updates in the console.
/// </summary>
internal sealed class ProgressConsoleMessageWriter
    : IAsyncLogMessageWriter<ProgressStartPayload, IAnsiConsole>
    , IAsyncLogMessageWriter<ProgressFinishedPayload, IAnsiConsole>
{
    private readonly ProgressFinishedPayloadOnlyFilter progressFinishedPayloadOnlyFilter = new();

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

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressStartPayload payload)
    {
        if (logSinkDisabledSubscription is null)
        {
            logSinkDisabledSubscription = LogSink?.AddFilter(progressFinishedPayloadOnlyFilter);

            payload.SetProgress(Writer.Progress());
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressFinishedPayload payload)
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
