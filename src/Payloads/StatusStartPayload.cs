using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class StatusStartPayload
{
    private readonly TaskCompletionSource<Status> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;

    public void SetStatus(Status status)
        => taskCompletionSource.TrySetResult(status);

    public Task<Status> WaitForStatusAsync()
        => taskCompletionSource.Task;
}
