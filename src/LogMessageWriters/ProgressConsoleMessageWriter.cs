using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's progress bar to 
/// render progress updates in the console.
/// </summary>
internal sealed class ProgressConsoleMessageWriter(SpectreConsoleLogSink logSink)
    : IAsyncLogMessageWriter<ProgressStartPayload>
    , IAsyncLogMessageWriter<ProgressFinishedPayload>
{
    private IDisposable? logSinkDisabledSubscription;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public ValueTask WriteAsync(ILogMessage<ProgressStartPayload> logMessage, CancellationToken cancellationToken)
    {
        if (logSinkDisabledSubscription is null)
        {
            logSinkDisabledSubscription = logSink.AddFilter<ProgressFinishedPayload>(lm => lm.Payload is ProgressFinishedPayload);

            logMessage.Payload.SetProgress(logSink.Console.Progress());
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask WriteAsync(ILogMessage<ProgressFinishedPayload> logMessage, CancellationToken cancellationToken)
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
