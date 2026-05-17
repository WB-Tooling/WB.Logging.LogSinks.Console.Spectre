using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class LogMessageWidget : IRenderable
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static readonly Segment spaceSegment = new(" ");

    private static readonly Segment newLineSegment = new("   ↪ ", new Style(foreground: Color.Grey));

    private static readonly int newLineSegmentLength = newLineSegment.CellCount();

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
        throw new NotImplementedException();
    }

    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        int offset = 0;

        foreach (Segment segment in Render(Timestamp, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        if (offset > 0 && LogLevel is not null && LogLevel.Any())
        {
            yield return spaceSegment;

            offset += spaceSegment.CellCount();
        }

        foreach (Segment segment in Render(LogLevel, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        if (offset > 0 && Senders is not null && Senders.Any())
        {
            yield return spaceSegment;

            offset += spaceSegment.CellCount();
        }

        foreach (Segment segment in Render(Senders, options, maxWidth - offset))
        {
            yield return segment;

            offset += segment.CellCount();
        }

        if (offset > 0 && Payload is not null && Payload.Any())
        {
            yield return spaceSegment;

            offset += spaceSegment.CellCount();
        }

        int remainingWidth = maxWidth - offset;

        Segment[] payloadSegments = Payload?.SelectMany(renderable => renderable.Render(options, int.MaxValue))?.ToArray() ?? [];

        int i = 0;

        for (i = 0; i < payloadSegments.Length; i++)
        {
            Segment segment = payloadSegments[i];

            if (segment.CellCount() < remainingWidth)
            {
                yield return segment;

                offset += segment.CellCount();
                remainingWidth = maxWidth - offset;
            }
            else
            {
                break;
            }
        }

        int lineWidth = maxWidth - newLineSegmentLength;
        bool newLine = offset > 0;

        List<SegmentLine> segmentLines = Segment.SplitLines(payloadSegments[i..], lineWidth);

        foreach (SegmentLine segmentLine in segmentLines)
        {
            if (newLine)
            {
                yield return Segment.LineBreak;
                yield return newLineSegment;
            }

            foreach (Segment lineSegment in segmentLine)
            {
                yield return lineSegment;
            }

            newLine = true;
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
