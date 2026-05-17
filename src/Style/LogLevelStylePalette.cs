using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// The <see cref="LogLevelStylePalette"/> record defines a set of styles for rendering 
/// different log levels in the console using Spectre.Console.
/// </summary>
/// <param name="DebugTextStyle"></param>
/// <param name="InfoTextStyle"></param>
/// <param name="WarningTextStyle"></param>
/// <param name="ErrorTextStyle"></param>
/// <param name="UnknownTextStyle"></param>
/// <param name="NoneTextStyle"></param>
/// <param name="BracketStyle"></param>
public readonly record struct LogLevelStylePalette(
    Style DebugTextStyle,
    Style InfoTextStyle,
    Style WarningTextStyle,
    Style ErrorTextStyle,
    Style NoneTextStyle,
    Style UnknownTextStyle,
    Style BracketStyle)
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets the default <see cref="LogLevelStylePalette"/> with predefined styles.
    /// </summary>
    public static LogLevelStylePalette Default { get; } = new LogLevelStylePalette(
        DebugTextStyle: new Style(foreground: Color.Gray),
        InfoTextStyle: new Style(foreground: Color.Green),
        WarningTextStyle: new Style(foreground: Color.Yellow),
        ErrorTextStyle: new Style(foreground: Color.Red),
        NoneTextStyle: new Style(foreground: Color.White, background: Color.Grey),
        UnknownTextStyle: new Style(foreground: Color.Magenta),
        BracketStyle: new Style(foreground: Color.White));
}
