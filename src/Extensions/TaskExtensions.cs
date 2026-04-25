using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal static class TaskExtensions
{
    internal static async Task StartAsync(this Task<Progress> @this, Func<ProgressContext, CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        Progress progress = await @this.ConfigureAwait(false);

        await progress.StartAsync(context => action(context, cancellationToken)).ConfigureAwait(false);
    }
}
