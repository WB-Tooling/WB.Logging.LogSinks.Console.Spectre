using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// Represents a payload for logging progress using Spectre.Console's progress bars.
/// </summary>
internal sealed class StatusPayload
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    private readonly TaskCompletionSource taskCompletionSource = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public required string StatusMessage { get; init; }

    /// <summary>
    /// Gets or sets the action that performs the status update.
    /// </summary>
    public required Func<StatusContext, Task> StatusAction { get; init; }

    public Action<Status> StatusConfigurationAction { get; init; } = _ => { };

    /// <summary>
    /// Gets a <see cref="Task"/> that represents the completion of the status. The 
    /// <see cref="Task"/> will complete when the status is completed or if an exception 
    /// occurs during the status update. The <see cref="Task"/> will complete successfully if 
    /// the status update completes successfully, or with an <see cref="Exception"/> if an <see cref="Exception"/> 
    /// occurs during the status update.
    /// </summary>
    public Task Completed => taskCompletionSource.Task;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Sets the status as completed, which will complete the <see cref="Completed"/> 
    /// task. This method should be called when the status is completed successfully.
    /// </summary>
    public void SetCompleted()
        => taskCompletionSource.TrySetResult();

    /// <summary>
    /// Sets the status as failed, which will complete the <see cref="Completed"/> 
    /// task with an exception. This method should be called when an error occurs during the status update.
    /// </summary>
    /// <param name="exception">The exception that caused the status update to fail.</param>
    public void SetException(Exception exception)
        => taskCompletionSource.TrySetException(exception);
}
