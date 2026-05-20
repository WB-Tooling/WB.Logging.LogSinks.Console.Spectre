using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class ProgressPayload
{
    public required Func<IAnsiConsole, Task> ExecuteAsync { get; init; }
}
