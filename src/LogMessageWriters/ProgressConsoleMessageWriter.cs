using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's progress bar to 
/// render progress updates in the console.
/// </summary>
internal sealed class ProgressConsoleMessageWriter
    : IAsyncLogMessageWriter<SpectreConsoleLogSink, ProgressStartPayload>
    , IAsyncLogMessageWriter<SpectreConsoleLogSink, ProgressFinishedPayload>
{
    private IDisposable? logSinkDisabledSubscription;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    [NotNull]
    public SpectreConsoleLogSink? LogSink { get; set; }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressStartPayload payload)
    {
        if (logSinkDisabledSubscription is null)
        {
            logSinkDisabledSubscription = LogSink.AddFilter<object>(lm => lm.Payload is ProgressFinishedPayload);

            payload.SetProgress(LogSink.Console.Progress());
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
