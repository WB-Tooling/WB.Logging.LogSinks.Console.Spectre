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
    /// Starts a progress with the <paramref name="progressConfiguration"/> running the specified <paramref name="action"/> function. 
    /// The progress will be automatically completed when the <paramref name="action"/> function completes.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to start the progress on.</param>
    /// <param name="progressConfiguration">The configuration for the progress. This includes settings such as
    /// <param name="action">The function that performs the progress.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <seealso cref="ProgressConfiguration"/>
    public static async Task StartProgressAsync(this ILogger @this, Action<Progress> progress, Func<ProgressContext, CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);

        ProgressPayload progressPayload = new()
        {
            ProgressConfigurationAction = progress,
            ProgressAction = action,
            CancellationToken = cancellationToken,
        };

        await @this.FlushAsync(cancellationToken).ConfigureAwait(false);

        @this.Log(null, progressPayload);

        await progressPayload.Completed.ConfigureAwait(false);
    }

    /// <summary>
    /// Starts a progress running the specified <paramref name="action"/> function. 
    /// The progress will be automatically completed when the <paramref name="action"/> function completes.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to start the progress on.</param>
    /// <param name="action">The function that performs the progress.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task StartProgressAsync(this ILogger @this, Func<ProgressContext, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        => await StartProgressAsync(@this, _ => { }, action, cancellationToken).ConfigureAwait(false);

    public static async Task StartStatusAsync(this ILogger @this, string message, Action<Status> status, Func<StatusContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(@this);

        StatusPayload statusPayload = new()
        {
            StatusConfigurationAction = status,
            StatusAction = action,
            StatusMessage = message,
        };

        await @this.FlushAsync().ConfigureAwait(false);

        @this.Log(null, statusPayload);

        await statusPayload.Completed.ConfigureAwait(false);
    }

    public static Task StartStatusAsync(this ILogger @this, string statusMessage, Func<StatusContext, Task> action)
        => StartStatusAsync(@this, statusMessage, _ => { }, action);
}
