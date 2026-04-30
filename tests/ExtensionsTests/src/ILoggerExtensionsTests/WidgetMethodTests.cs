using System;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.WidgetMethodTests;

public static class DataProvider
{
    public static Func<IRenderable> TextWidget => () => new Text("Hello, World!");
    public static Func<IRenderable> GridWidget => () =>
    {
        Grid grid = new();

        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow("Hello", "World");
        grid.AddRow("Goodbye", "Everyone");

        return grid;
    };

    public static Func<IRenderable> FigletWidget => () => new FigletText("Hello, World!");
}

public sealed class TheWidgetMethod
{
    [Test]
    [MethodDataSource(typeof(DataProvider), nameof(DataProvider.TextWidget))]
    [MethodDataSource(typeof(DataProvider), nameof(DataProvider.GridWidget))]
    [MethodDataSource(typeof(DataProvider), nameof(DataProvider.FigletWidget))]
    public async Task ShouldLogTheWidgetAsALogMessage(IRenderable widget)
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true,
        };
        Logger logger = new("Test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Console = testConsole;
        });

        // Act
        logger.Widget(widget);
        await logger.FlushAsync().ConfigureAwait(false);

        // Assert
        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }
}
