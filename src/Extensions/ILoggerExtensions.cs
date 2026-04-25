using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// Provides extension methods for the <see cref="ILogger"/> interface.
/// </summary>
public static class ILoggerExtensions
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Attaches a <see cref="SpectreConsoleLogSink"/> to <paramref name="this"/> <see cref="ILogger"/>.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to attach the log sink to.</param>
    /// <param name="configure">An optional action to configure the <see cref="SpectreConsoleLogSink"/> after it has been created and attached.</param>
    /// <returns>An <see cref="IDisposable"/> that can be used to detach the log sink from the logger when it is no longer needed.</returns>
    public static IDisposable AttachSpectreConsole(this ILogger @this, Action<SpectreConsoleLogSink>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(@this);

        SpectreConsoleLogSink logSink = new();

        IDisposable disposable = @this.AttachLogSink(logSink);

        configure?.Invoke(logSink);

        return disposable;
    }

    /// <summary>
    /// Logs the <paramref name="widget"/> as a log message. This method allows you to log any 
    /// Spectre.Console widget directly by passing it as an argument.
    /// </summary>
    /// <remarks>The <paramref name="widget"/> is logged with a <c>null</c> <see cref="LogLevel"/>.</remarks>
    /// <param name="this">The <see cref="ILogger"/> instance to log the widget to.</param>
    /// <param name="widget">The Spectre.Console widget to log.</param>
    /// <returns>The same <see cref="ILogger"/> instance to allow for method chaining.</returns>
    /// <seealso cref="IRenderable"/>
    public static ILogger Widget(this ILogger @this, IRenderable widget)
    {
        ArgumentNullException.ThrowIfNull(@this);

        @this.Log(null, new WidgetPayload(widget));

        return @this;
    }

    /// <summary>
    /// Logs a horizontal rule to the console.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to log the horizontal rule to.</param>
    /// <param name="title">An optional title to display in the center of the horizontal rule.</param>
    /// <returns>The same <see cref="ILogger"/> instance to allow for method chaining.</returns>
    public static ILogger HorizontalRule(this ILogger @this, string? title = null)
    {
        ArgumentNullException.ThrowIfNull(@this);

        if (title is null)
        {
            return @this.Widget(new Rule());
        }
        else
        {
            @this.Widget(new Rule(title));
        }

        return @this;
    }

    /// <summary>
    /// Logs a <see cref="Progress"/> to the console.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to log the progress to.</param>
    /// <param name="action">An action that receives the <see cref="Progress"/> instance to configure it and add tasks to it.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the progress operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous progress operation.</returns>
    public static async Task ProgressAsync(this ILogger @this, Func<Progress, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(action);

        ProgressStartPayload startPayload = new()
        {
            CancellationToken = cancellationToken,
        };

        @this.Log(null, startPayload);

        Progress progress = await startPayload.WaitForProgressAsync().ConfigureAwait(false);

        await action(progress).ConfigureAwait(false);

        ProgressFinishedPayload finishedPayload = new()
        {
            CancellationToken = cancellationToken,
        };

        @this.Log(null, finishedPayload);

        await finishedPayload.WaitForFinishedAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Logs a <see cref="Status"/> to the console.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to log the status to.</param>
    /// <param name="action">An action that receives the <see cref="Status"/> instance to configure it and add tasks to it.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the status operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous status operation.</returns>
    public static async Task StatusAsync(this ILogger @this, Func<Status, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(action);

        StatusStartPayload startPayload = new()
        {
            CancellationToken = cancellationToken,
        };

        @this.Log(null, startPayload);

        Status status = await startPayload.WaitForStatusAsync().ConfigureAwait(false);

        await action(status).ConfigureAwait(false);

        StatusFinishedPayload finishedPayload = new()
        {
            CancellationToken = cancellationToken,
        };

        @this.Log(null, finishedPayload);

        await finishedPayload.WaitForFinishedAsync().ConfigureAwait(false);
    }
}
