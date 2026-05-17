using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

/// <summary>
/// A base class for log message writers that use Spectre.Console 
/// to render log messages in the console. This class provides properties to 
/// control the visibility of different parts of log messages (timestamp, log level, senders, payload) and methods 
/// to render these parts. Derived classes can override the rendering methods to customize the appearance of log messages.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class SpectreConsoleLogMessageWriter<TValue> : IAsyncLogMessageWriter<TValue>
    where TValue : notnull
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private readonly SpectreConsoleLogSink logSink;

    private readonly BadgeWidget debugBadge;

    private readonly BadgeWidget infoBadge;

    private readonly BadgeWidget warningBadge;

    private readonly BadgeWidget errorBadge;

    private readonly BadgeWidget noneBadge;

    private readonly BadgeWidget unknownBadge;

    private readonly LogMessageWidget logMessageWidget = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Constructors                                                         │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreConsoleLogMessageWriter{TValue}"/> class with default styles for log levels. The constructor creates badge widgets for different log levels (debug, info, warning, error, none, unknown) 
    /// using the styles defined in the <see cref="StylePalette"/>. These badges are used to render the log level 
    /// part of log messages when the <see cref="ShowLogLevel"/> property is set to <c>true</c>.
    /// </summary>
    public SpectreConsoleLogMessageWriter(SpectreConsoleLogSink logSink)
    {
        this.logSink = logSink ?? throw new ArgumentNullException(nameof(logSink));

        debugBadge = new BadgeWidget("DEBU")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.DebugTextStyle,
        };

        infoBadge = new BadgeWidget("INFO")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.InfoTextStyle,
        };

        warningBadge = new BadgeWidget("WARN")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.WarningTextStyle,
        };

        errorBadge = new BadgeWidget("ERRO")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.ErrorTextStyle,
        };

        noneBadge = new BadgeWidget("NONE")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.NoneTextStyle,
        };

        unknownBadge = new BadgeWidget("UNKN")
        {
            BracketStyle = StylePalette.LogLevelStyle.BracketStyle,
            TextStyle = StylePalette.LogLevelStyle.UnknownTextStyle,
        };
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets or sets the <see cref="StylePalette"/> used to style different parts of log messages.
    /// </summary>
    public StylePalette StylePalette { get; init; } = StylePalette.Default;

    /// <summary>
    /// Gets or sets a value indicating whether to show the timestamp of log messages. Default is <c>true</c>.
    /// </summary>
    public bool ShowTimestamp { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show the log level of log messages. Default is <c>true</c>.
    /// </summary>
    public bool ShowLogLevel { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show the senders of log messages. Default is <c>true</c>.
    /// </summary>
    public bool ShowSenders { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to show the payload of log messages. Default is <c>true</c>.
    /// </summary>
    public bool ShowPayload { get; set; } = true;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public virtual ValueTask WriteAsync(ILogMessage<TValue> logMessage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(logMessage);

        logMessageWidget.Reset();

        logMessageWidget.Timestamp = ShowTimestamp ? RenderTimestamp(logMessage.Timestamp) : null;
        logMessageWidget.LogLevel = ShowLogLevel ? RenderLogLevel(logMessage.LogLevel) : null;
        logMessageWidget.Senders = ShowSenders && logMessage.Senders.Count > 0 ? RenderSenders(logMessage.Senders) : null;
        logMessageWidget.Payload = ShowPayload ? RenderPayload(logMessage.Payload) : null;

        logSink.Console.Write(logMessageWidget);

        return ValueTask.CompletedTask;
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Renders the timestamp part of a log message.
    /// </summary>
    /// <remarks>This method can be overridden in derived classes to customize the format and style of the rendered timestamp.</remarks>
    /// <param name="timestamp">The timestamp to render.</param>
    /// <returns>An enumerable of <see cref="IRenderable"/> representing the rendered timestamp.</returns>
    protected virtual IEnumerable<IRenderable> RenderTimestamp(DateTimeOffset timestamp)
    {
        yield return new Markup(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", global::System.Globalization.CultureInfo.InvariantCulture), StylePalette.TimestampStyle);
    }

    /// <summary>
    /// Renders the log level part of a log message.
    /// </summary>
    /// <remarks>This method can be overridden in derived classes to customize the format and style of the rendered log level.</remarks>
    /// <param name="logLevel">The <see cref="LogLevel"/> to render.</param>
    /// <returns>An enumerable of <see cref="IRenderable"/> representing the rendered <see cref="LogLevel"/>.</returns>
    /// <seealso cref="LogLevel"/>
    protected virtual IEnumerable<IRenderable> RenderLogLevel(LogLevel? logLevel)
    {
        yield return logLevel switch
        {
            LogLevel.Info => infoBadge,
            LogLevel.Warning => warningBadge,
            LogLevel.Error => errorBadge,
            null => noneBadge,
            _ => unknownBadge
        };
    }

    /// <summary>
    /// Renders the senders part of a log message.
    /// </summary>
    /// <remarks>This method can be overridden in derived classes to customize the format and style of the rendered senders.</remarks>
    /// <param name="senders">The senders to render.</param>
    /// <returns>An enumerable of <see cref="IRenderable"/> representing the rendered senders.</returns>
    protected virtual IEnumerable<IRenderable> RenderSenders(IEnumerable<string> senders)
    {
        ArgumentNullException.ThrowIfNull(senders);

        IEnumerable<IRenderable> senderBadges = senders.Select(sender => new BadgeWidget(sender)
        {
            TextStyle = StylePalette.SendersStyle
        });

        yield return new Columns(senderBadges)
        {
            Expand = false,
        };
    }

    /// <summary>
    /// Renders the payload part of a log message.
    /// </summary>
    /// <remarks>This method can be overridden in derived classes to customize the format and style of the rendered payload.</remarks>
    /// <param name="payload">The payload to render.</param>
    /// <returns>An enumerable of <see cref="IRenderable"/> representing the rendered payload.</returns>
    protected virtual IEnumerable<IRenderable> RenderPayload(TValue payload)
    {
        yield return payload.ToRenderable();
    }
}
