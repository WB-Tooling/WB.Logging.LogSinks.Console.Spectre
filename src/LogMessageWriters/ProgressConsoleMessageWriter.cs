using System;
using System.Threading;
using System.Threading.Tasks;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's progress bar to 
/// render progress updates in the console.
/// </summary>
internal sealed class ProgressConsoleMessageWriter(SpectreConsoleLogSink logSink)
    : IAsyncLogMessageWriter<ProgressPayload>
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public async ValueTask WriteAsync(ILogMessage<ProgressPayload> logMessage, CancellationToken cancellationToken)
    {
        using IDisposable filter = logSink.Disable();

        await logMessage.Payload.ExecuteAsync(logSink.Console).ConfigureAwait(false);
    }
}
