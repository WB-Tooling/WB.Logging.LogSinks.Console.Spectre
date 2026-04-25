using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log sink that writes <see cref="ILogMessage{TPayload}"/> to the console using 
/// Spectre.Console. This log sink is designed to be attached to an <see cref="ILogger"/> 
/// instance, allowing you to log messages with rich formatting and interactive widgets provided by Spectre.Console.
/// </summary>
public sealed class SpectreConsoleLogSink : AsyncLogSinkBase<IAnsiConsole>
{
    private readonly ProgressConsoleMessageWriter progressConsoleMessageWriter = new();

    private readonly StatusConsoleMessageWriter statusConsoleMessageWriter = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                         │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreConsoleLogSink"/> class.
    /// </summary>
    public SpectreConsoleLogSink() : base(new SpectreConsoleLogMessageWriter<object>(), AnsiConsole.Console)
    {
        RegisterLogMessageWriter(new PayloadLogMessageWriter());
        RegisterLogMessageWriter<ProgressStartPayload>(progressConsoleMessageWriter);
        RegisterLogMessageWriter<ProgressFinishedPayload>(progressConsoleMessageWriter);
        RegisterLogMessageWriter<StatusStartPayload>(statusConsoleMessageWriter);
        RegisterLogMessageWriter<StatusFinishedPayload>(statusConsoleMessageWriter);
    }
}
