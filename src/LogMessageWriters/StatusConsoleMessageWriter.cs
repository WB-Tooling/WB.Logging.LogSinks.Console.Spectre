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
internal sealed class StatusConsoleMessageWriter(SpectreConsoleLogSink logSink)
    : IAsyncLogMessageWriter<StatusPayload>
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public async ValueTask WriteAsync(ILogMessage<StatusPayload> logMessage, CancellationToken cancellationToken)
    {
        using IDisposable filter = logSink.Disable();

        await logMessage.Payload.ExecuteAsync(logSink.Console).ConfigureAwait(false);
    }
}
