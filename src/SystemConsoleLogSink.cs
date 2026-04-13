using WB.Logging.LogSinks.Base;
using WB.Logging.LogSinks.Console.Spectre;

namespace WB.Logging.LogSinks.Console.System;

public sealed class SystemConsoleLogSink : AsyncLogSinkBase
{
    public SystemConsoleLogSink() : base(new SpectreConsoleLogMessageWriter<object>())
    {
    }
}
