using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressStartPayload
{
    private readonly TaskCompletionSource<Progress> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;

    public void SetProgress(Progress progress)
        => taskCompletionSource.TrySetResult(progress);

    public Task<Progress> WaitForProgressAsync()
        => taskCompletionSource.Task;
}
