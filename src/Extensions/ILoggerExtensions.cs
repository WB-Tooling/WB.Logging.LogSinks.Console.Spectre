using System;
using System.Diagnostics.CodeAnalysis;
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
    public static IAsyncDisposable AttachSpectreConsole(this ILogger @this, Action<SpectreConsoleLogSink>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(@this);

        SpectreConsoleLogSink logSink = new();

        IDisposable disposable = @this.AttachLogSink(logSink);

        configure?.Invoke(logSink);

        return new ActionDisposable(async () =>
        {
            disposable.Dispose();
            await logSink.DisposeAsync().ConfigureAwait(false);
        });
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
    /// <param name="action">An action that receives the <see cref="ProgressContext"/> instance to configure it and add tasks to it.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the progress operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous progress operation.</returns>
    public static Task ProgressAsync(this ILogger @this, Func<ProgressContext, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);

        TaskCompletionSource taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        ProgressPayload payload = new()
        {
            ExecuteAsync = async console =>
            {
                try
                {
                    await console.Progress().StartAsync(action).ConfigureAwait(false);

                    taskCompletionSource.SetResult();
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            },
        };

        @this.Log(null, payload);

        return taskCompletionSource.Task;
    }

    /// <summary>
    /// Logs a <see cref="Status"/> to the console.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> instance to log the status to.</param>
    /// <param name="status">The status text to display.</param>
    /// <param name="action">An action that receives the <see cref="Status"/> instance to configure it and add tasks to it.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the status operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous status operation.</returns>
    public static Task StatusAsync(this ILogger @this, string status, Func<StatusContext, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(action);

        StatusStartPayload startPayload = new()
        {
            CancellationToken = cancellationToken,
        };

        TaskCompletionSource taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        StatusPayload statusPayload = new()
        {
            ExecuteAsync = async console =>
            {
                try
                {
                    await console.Status().StartAsync(status, action).ConfigureAwait(false);

                    taskCompletionSource.SetResult();
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            },
        };

        @this.Log(null, statusPayload);

        return taskCompletionSource.Task;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We want to catch all exceptions to set them on the payload.")]
    public static Task<bool> ConfirmAsync(this ILogger @this, string prompt, bool defaultValue = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(prompt);

        TaskCompletionSource<bool> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        ConfirmPayload payload = new()
        {
            ExecuteAsync = async console =>
            {
                try
                {
                    bool result = await console.ConfirmAsync(prompt, defaultValue, cancellationToken: cancellationToken).ConfigureAwait(false);

                    taskCompletionSource.SetResult(result);
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            },
        };

        @this.Log(null, payload);

        return taskCompletionSource.Task;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "We want to catch all exceptions to set them on the payload.")]
    public static Task<T> AskAsync<T>(this ILogger @this, string prompt, CancellationToken cancellationToken = default)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(@this);
        ArgumentNullException.ThrowIfNull(prompt);

        TaskCompletionSource<T> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

        AskPayload payload = new()
        {
            ExecuteAsync = async console =>
            {
                try
                {
                    T result = await console.AskAsync<T>(prompt, cancellationToken: cancellationToken).ConfigureAwait(false);

                    taskCompletionSource.TrySetResult(result);
                }
                catch (Exception exception)
                {
                    taskCompletionSource.TrySetException(exception);
                }
            },
        };

        @this.Log(null, payload);

        return taskCompletionSource.Task;
    }
}
