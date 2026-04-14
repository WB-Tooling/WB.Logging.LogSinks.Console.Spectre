using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

public sealed class SpectreConsoleLogSink : AsyncLogSinkBase
{
    public SpectreConsoleLogSink() : base(new SpectreConsoleLogMessageWriter<object>())
    {
    }
}
