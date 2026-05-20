using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class StatusPayload
{
    public required Func<IAnsiConsole, Task> ExecuteAsync { get; init; }
}
