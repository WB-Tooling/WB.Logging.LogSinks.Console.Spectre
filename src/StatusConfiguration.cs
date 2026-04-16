using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

public sealed record StatusConfiguration(
    string StatusMessage,
    bool AutoRefresh = true)
{
    public Spinner Spinner { get; init; } = Spinner.Known.Dots;
}
