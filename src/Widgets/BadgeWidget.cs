using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class BadgeWidget(string text) : IRenderable
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private readonly string text = text;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public Style BracketStyle { get; init; } = new Style(Color.White);

    public Style TextStyle { get; init; } = Style.Plain;

    public string OpeningBracket { get; init; } = "[";

    public string ClosingBracket { get; init; } = "]";

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public Measurement Measure(RenderOptions options, int maxWidth)
        => new(0, text.Length + OpeningBracket.Length + ClosingBracket.Length);

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        yield return new Segment(OpeningBracket, BracketStyle);
        yield return new Segment(text, TextStyle);
        yield return new Segment(ClosingBracket, BracketStyle);
    }
}