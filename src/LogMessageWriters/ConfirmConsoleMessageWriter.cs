using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ConfirmConsoleMessageWriter(SpectreConsoleLogSink logSink) : IAsyncLogMessageWriter<ConfirmPayload>
{
    public async ValueTask WriteAsync(ILogMessage<ConfirmPayload> logMessage, CancellationToken cancellationToken)
    {
        using IDisposable filter = logSink.Disable();

        await logMessage.Payload.ExecuteAsync(logSink.Console).ConfigureAwait(false);
    }
}
