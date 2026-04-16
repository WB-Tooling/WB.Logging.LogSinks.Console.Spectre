namespace WB.Logging.LogSinks.Console.Spectre;

public sealed record ProgressConfiguration(
    bool AutoClear = true,
    bool AutoRefresh = true,
    bool HideCompleted = true);
