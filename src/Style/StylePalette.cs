using Spectre.Console;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// The <see cref="StylePalette"/> record defines a set of styles for rendering 
/// different parts of log messages in the console using Spectre.Console. It includes 
/// styles for timestamps, log levels, and senders. The default styles are defined 
/// in the static property <see cref="Default"/>. This record can be used to customize 
/// the appearance of log messages by providing different styles when creating an 
/// instance of <see cref="SpectreConsoleLogMessageWriter{TValue}"/>.
/// </summary>
/// <param name="TimestampStyle"></param>
/// <param name="LogLevelStyle"></param>
/// <param name="SendersStyle"></param>
public sealed record StylePalette(
    Style TimestampStyle,
    LogLevelStylePalette LogLevelStyle,
    Style SendersStyle)
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets the default <see cref="StylePalette"/> with predefined 
    /// styles for timestamps, log levels, and senders. The default styles are:
    /// </summary>
    public static StylePalette Default { get; } = new StylePalette(
        TimestampStyle: new Style(foreground: Color.Grey),
        LogLevelStyle: LogLevelStylePalette.Default,
        SendersStyle: new Style(foreground: Color.Blue));
}
