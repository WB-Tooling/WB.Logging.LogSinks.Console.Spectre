using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressFinishedPayload
{
    private readonly TaskCompletionSource taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;

    public void SetFinished()
        => taskCompletionSource.TrySetResult();

    public Task WaitForFinishedAsync()
        => taskCompletionSource.Task;
}
