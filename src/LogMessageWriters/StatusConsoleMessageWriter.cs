using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's status to 
/// render status updates in the console.
/// </summary>
internal sealed class StatusConsoleMessageWriter
    : IAsyncLogMessageWriter<StatusStartPayload>
    , IAsyncLogMessageWriter<StatusFinishedPayload>
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

    public ValueTask WriteAsync(ILogMessage<StatusStartPayload> logMessage, CancellationToken cancellationToken)
    {
        if (logSinkDisabledSubscription is null && LogSink is not null)
        {
            logSinkDisabledSubscription = LogSink.AddFilter<object>(lm => lm.Payload is StatusFinishedPayload);

            logMessage.Payload.SetStatus(LogSink.Console.Status());
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteAsync(ILogMessage<StatusFinishedPayload> logMessage, CancellationToken cancellationToken)
    {
        if (logSinkDisabledSubscription is not null)
        {
            logSinkDisabledSubscription.Dispose();
            logSinkDisabledSubscription = null;

            logMessage.Payload.SetFinished();
        }

        return ValueTask.CompletedTask;
    }
}
