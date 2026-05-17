using System.Threading;
using System.Threading.Tasks;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class WidgetPayloadLogMessageWriter(SpectreConsoleLogSink logSink) : IAsyncLogMessageWriter<WidgetPayload>
{
    public ValueTask WriteAsync(ILogMessage<WidgetPayload> logMessage, CancellationToken cancellationToken)
    {
        logSink.Console.Write(logMessage.Payload.Widget);

        return ValueTask.CompletedTask;
    }
}
