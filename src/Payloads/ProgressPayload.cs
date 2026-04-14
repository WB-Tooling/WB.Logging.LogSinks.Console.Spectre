using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

public sealed class ProgressPayload
{
    private readonly TaskCompletionSource taskCompletionSource = new();

    public required string Title { get; init; }

    public required Func<ProgressContext, Task> Progress { get; init; }

    public bool AutoClear { get; init; } = true;

    public bool AutoRefresh { get; init; } = true;

    public bool HideCompleted { get; init; } = true;

    public Task Completed => taskCompletionSource.Task;

    public void SetCompleted()
        => taskCompletionSource.TrySetResult();

    public void SetException(Exception exception)
        => taskCompletionSource.TrySetException(exception);
}
