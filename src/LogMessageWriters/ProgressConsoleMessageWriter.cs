using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A log message writer that uses Spectre.Console's progress bar to 
/// render progress updates in the console.
/// </summary>
internal sealed class ProgressConsoleMessageWriter : IAsyncLogMessageWriter<ProgressPayload, IAnsiConsole>
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public IAnsiConsole Writer { get; set; } = AnsiConsole.Console;

    /// <inheritdoc/>
    public IAsyncLogSink? LogSink { get; set; }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public async ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, ProgressPayload payload)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await Writer.Progress()
                .AutoClear(payload.AutoClear)
                .AutoRefresh(payload.AutoRefresh)
                .HideCompleted(payload.HideCompleted)
                .StartAsync(payload.Progress).ConfigureAwait(false);

            payload.SetCompleted();
        }
        catch (Exception exception)
        {
            payload.SetException(exception);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
