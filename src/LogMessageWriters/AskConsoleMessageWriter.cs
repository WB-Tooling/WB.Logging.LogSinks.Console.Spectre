using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class AskConsoleMessageWriter(SpectreConsoleLogSink logSink) : IAsyncLogMessageWriter<AskPayload>
{
    public async ValueTask WriteAsync(ILogMessage<AskPayload> logMessage, CancellationToken cancellationToken)
    {
        using IDisposable filter = logSink.Disable();

        await logMessage.Payload.ExecuteAsync(logSink.Console).ConfigureAwait(false);
    }
}
