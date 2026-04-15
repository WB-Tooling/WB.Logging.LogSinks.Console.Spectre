using Spectre.Console.Rendering;

namespace WB.Logging.LogSinks.Console.Spectre;

internal sealed class WidgetPayload(IRenderable renderable)
{
    public IRenderable Widget { get; } = renderable;
}
