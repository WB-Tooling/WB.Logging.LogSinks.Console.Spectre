using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ConfirmPayload
{
    public required Func<IAnsiConsole, Task> ExecuteAsync { get; init; }
}
