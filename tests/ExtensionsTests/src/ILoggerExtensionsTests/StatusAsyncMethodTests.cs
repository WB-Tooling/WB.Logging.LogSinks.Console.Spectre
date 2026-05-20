using System.Threading.Tasks;
using AwesomeAssertions;
using Spectre.Console;
using Spectre.Console.Testing;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.StatusAsyncMethodTests;

public sealed class TheStatusAsyncMethod
{
    [Test]
    public async Task ShouldRunAStatus()
    {
        // Arrange
        TestConsole testConsole = new();
        ILogger logger = new Logger("test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Console = testConsole;
        });

        // Act
        await logger.StatusAsync("status", async context =>
        {
            for (int i = 0; i < 100; i++)
            {
                context.Status($"Processing... {i}%");
                await Task.Delay(10);
            }
        }).ConfigureAwait(false);
        await logger.FlushAsync();

        // Assert
        testConsole.Output.Should().Contain("Processing...");
    }
}
