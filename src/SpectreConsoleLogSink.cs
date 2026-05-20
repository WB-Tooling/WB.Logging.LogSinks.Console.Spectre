using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log sink that writes <see cref="ILogMessage{TPayload}"/> to the console using 
/// Spectre.Console. This log sink is designed to be attached to an <see cref="ILogger"/> 
/// instance, allowing you to log messages with rich formatting and interactive widgets provided by Spectre.Console.
/// </summary>
public sealed class SpectreConsoleLogSink : AsyncLogSinkBase<SpectreConsoleLogSink>
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private readonly LogMessageFilterPipeline logMessageFilterPipeline = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                         │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreConsoleLogSink"/> class.
    /// </summary>
    [SetsRequiredMembers]
    public SpectreConsoleLogSink() : base()
    {
        DefaultLogMessageWriter = new SpectreConsoleLogMessageWriter<object>(this);

        RegisterLogMessageWriter<WidgetPayloadLogMessageWriter>();
        RegisterLogMessageWriter<ProgressConsoleMessageWriter>();
        RegisterLogMessageWriter<StatusConsoleMessageWriter>();
        RegisterLogMessageWriter<WidgetPayloadLogMessageWriter>();

        ServiceContainer.RegisterInstance(this, disposeWithContainer: false);
        ServiceContainer.RegisterInstance(Console, disposeWithContainer: false);
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Internal Properties                                                         │
    // └─────────────────────────────────────────────────────────────────────────────┘
    internal IAnsiConsole Console { get; set; } = AnsiConsole.Console;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public override ValueTask SubmitAsync<TPayload>(ILogMessage<TPayload> logMessage, CancellationToken cancellationToken)
        => logMessageFilterPipeline.IsMatch(logMessage) ? base.SubmitAsync(logMessage, cancellationToken) : ValueTask.CompletedTask;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Internal Methods                                                            │
    // └─────────────────────────────────────────────────────────────────────────────┘
    internal IDisposable AddFilter(LogMessageFilter filter)
        => logMessageFilterPipeline.Add(filter);
}
