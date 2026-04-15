using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class LogMessageWidget : IRenderable
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static readonly Segment spaceSegment = new(" ");

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Properties                                                           │
    // └─────────────────────────────────────────────────────────────────────────────┘
    public IEnumerable<IRenderable>? Timestamp { get; set; } = [];

    public IEnumerable<IRenderable>? LogLevel { get; set; } = [];

    public IEnumerable<IRenderable>? Senders { get; set; } = [];

    public IEnumerable<IRenderable>? Payload { get; set; } = [];

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        throw new global::System.NotImplementedException();
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        int offset = 0;

        foreach (Segment segment in Render(Timestamp, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        yield return spaceSegment;

        offset += spaceSegment.CellCount();

        foreach (Segment segment in Render(LogLevel, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        yield return spaceSegment;

        offset += spaceSegment.CellCount();

        foreach (Segment segment in Render(Senders, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        yield return spaceSegment;

        offset += spaceSegment.CellCount();

        int remainingWidth = maxWidth - offset;

        if (remainingWidth < 10)
        {
            yield return Segment.LineBreak;

            offset = 0;
        }

        foreach (IRenderable renderable in Payload ?? [])
        {
            Segment[] segments = [.. renderable.Render(options, maxWidth - offset)];

            if (remainingWidth < 10)
            {
                yield return Segment.Empty;

                if (segments.Length > 0)
                {
                    yield return segments[0];
                }

                if (segments.Length > 1)
                {
                    Segment padding = new(new string(' ', offset));

                    for (int i = 1; i < segments.Length; i++)
                    {
                        yield return padding;
                        yield return segments[i];
                    }
                }
            }
            else
            {
                if (segments.Length > 0)
                {
                    yield return segments[0];
                }

                if (segments.Length > 1)
                {
                    Segment padding = new(new string(' ', offset));

                    for (int i = 1; i < segments.Length; i++)
                    {
                        yield return padding;
                        yield return segments[i];
                    }
                }
            }
        }
    }

    public void Reset()
    {
        Timestamp = null;
        LogLevel = null;
        Senders = null;
        Payload = null;
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Methods                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static IEnumerable<Segment> Render(IEnumerable<IRenderable>? renderables, RenderOptions options, int maxWidth)
    {
        if (renderables is null)
        {
            yield break;
        }
        else
        {
            foreach (IRenderable renderable in renderables)
            {
                foreach (Segment segment in renderable.Render(options, maxWidth))
                {
                    yield return segment;
                }
            }
        }
    }
}
