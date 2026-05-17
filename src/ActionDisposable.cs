using System;
using System.Threading.Tasks;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ActionDisposable(Func<Task> action) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
        => await action().ConfigureAwait(false);
}
