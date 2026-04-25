namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressFinishedPayloadOnlyFilter : ILogMessageFilter
{
    public bool IsMatch<TPayload>(ILogMessage<TPayload> logMessage) where TPayload : notnull
        => logMessage.Payload is ProgressFinishedPayload;
}
