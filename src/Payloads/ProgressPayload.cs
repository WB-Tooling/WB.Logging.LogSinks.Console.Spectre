using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// Represents a payload for logging progress using Spectre.Console's progress bars.
/// </summary>
public sealed class ProgressPayload
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    private readonly TaskCompletionSource taskCompletionSource = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets or sets the title of the progress. 
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets or sets the function that performs the progress.
    /// </summary>
    public required Func<ProgressContext, Task> Progress { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress should be automatically cleared from the console when completed. Default is <c>true</c>.
    /// </summary>
    public bool AutoClear { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the progress should be automatically refreshed in the console. Default is <c>true</c>.
    /// </summary>
    public bool AutoRefresh { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to hide completed tasks in the progress. Default is <c>true</c>.
    /// </summary>
    public bool HideCompleted { get; init; } = true;

    /// <summary>
    /// Gets a <see cref="Task"/> that represents the completion of the progress. The 
    /// <see cref="Task"/> will complete when the progress is completed or if an exception 
    /// occurs during the progress. The <see cref="Task"/> will complete successfully if 
    /// the progress completes successfully, or with an <see cref="Exception"/> if an <see cref="Exception"/> 
    /// occurs during the progress.
    /// </summary>
    public Task Completed => taskCompletionSource.Task;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Sets the progress as completed, which will complete the <see cref="Completed"/> 
    /// task. This method should be called when the progress is completed successfully.
    /// </summary>
    public void SetCompleted()
        => taskCompletionSource.TrySetResult();

    /// <summary>
    /// Sets the progress as failed, which will complete the <see cref="Completed"/> 
    /// task with an exception. This method should be called when an error occurs during the progress.
    /// </summary>
    /// <param name="exception">The exception that caused the progress to fail.</param>
    public void SetException(Exception exception)
        => taskCompletionSource.TrySetException(exception);
}
