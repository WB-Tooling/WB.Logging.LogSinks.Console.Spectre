using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

public sealed record StylePalette(
    Style TimestampStyle,
    Style LogLevelInfoStyle,
    Style LogLevelWarningStyle,
    Style LogLevelErrorStyle,
    Style LogLevelUnknownStyle,
    Style SendersStyle)
{
    public static StylePalette Default { get; } = new StylePalette(
        TimestampStyle: new Style(foreground: Color.Grey),
        LogLevelInfoStyle: new Style(foreground: Color.Green),
        LogLevelWarningStyle: new Style(foreground: Color.Yellow),
        LogLevelErrorStyle: new Style(foreground: Color.Red),
        LogLevelUnknownStyle: new Style(foreground: Color.Magenta),
        SendersStyle: new Style(foreground: Color.Blue));
}