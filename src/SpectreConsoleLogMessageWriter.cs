using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;
using WB.Logging.LogSinks.Base;

namespace WB.Logging.LogSinks.Console.Spectre;

public class SpectreConsoleLogMessageWriter<TValue> : IAsyncLogMessageWriter<TValue>
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static readonly BadgeWidget infoBadge = new("INFO") { TextStyle = Style.Parse("green") };

    private static readonly BadgeWidget warningBadge = new("WARN") { TextStyle = Style.Parse("yellow") };

    private static readonly BadgeWidget errorBadge = new("ERRO") { TextStyle = Style.Parse("red") };

    private static readonly BadgeWidget unknownBadge = new("UNKN") { TextStyle = Style.Parse("grey") };

    private LogMessageWidget logMessageWidget = new();

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Internal Properties                                                         │
    // └─────────────────────────────────────────────────────────────────────────────┘

    internal IAnsiConsole AnsiConsole { get; set; } = global::Spectre.Console.AnsiConsole.Console;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc/>
    public virtual ValueTask WriteAsync(DateTimeOffset timestamp, LogLevel? logLevel, IEnumerable<string> senders, TValue? payload)
    {
        logMessageWidget.Reset();

        logMessageWidget.Timestamp = ShowTimestamp ? RenderTimestamp(timestamp) : null;
        logMessageWidget.LogLevel = ShowLogLevel ? RenderLogLevel(logLevel) : null;
        logMessageWidget.Senders = ShowSenders ? RenderSenders(senders) : null;
        logMessageWidget.Payload = ShowPayload ? RenderPayload(payload) : null;

        AnsiConsole.Write(logMessageWidget);

        return ValueTask.CompletedTask;
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public StylePalette StylePalette { get; set; } = StylePalette.Default;

    public bool ShowTimestamp { get; set; } = true;

    public bool ShowLogLevel { get; set; } = true;

    public bool ShowSenders { get; set; } = true;

    public bool ShowPayload { get; set; } = true;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Protected Methods                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    protected virtual IEnumerable<IRenderable> RenderTimestamp(DateTimeOffset timestamp)
    {
        yield return new Markup(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", global::System.Globalization.CultureInfo.InvariantCulture), StylePalette.TimestampStyle);
    }

    protected virtual IEnumerable<IRenderable> RenderLogLevel(LogLevel? logLevel)
    {
        if (logLevel is null)
        {
            yield break;
        }

        yield return logLevel switch
        {
            LogLevel.Info => infoBadge,
            LogLevel.Warning => warningBadge,
            LogLevel.Error => errorBadge,
            _ => unknownBadge
        };
    }

    protected virtual IEnumerable<IRenderable> RenderSenders(IEnumerable<string> senders)
    {
        ArgumentNullException.ThrowIfNull(senders);

        foreach (var sender in senders)
        {
            yield return new BadgeWidget(sender)
            {
                TextStyle = StylePalette.SendersStyle
            };
        }
    }

    protected virtual IEnumerable<IRenderable> RenderPayload(TValue? payload)
    {
        yield return payload.ToRenderable();
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Methods                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘
}