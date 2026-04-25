using System.Threading.Tasks;
using AwesomeAssertions;
using Spectre.Console;
using Spectre.Console.Testing;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.ProgressAsyncMethodTests;

public sealed class TheProgressAsyncMethod
{
    [Test]
    public async Task ShouldRunAProgress()
    {
        // Arrange
        TestConsole testConsole = new();
        ILogger logger = new Logger("test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Writer = testConsole;
        });

        // Act
        await logger.ProgressAsync(async progress =>
        {
            await progress.StartAsync(async context =>
            {
                ProgressTask task = context.AddTask("Processing...").MaxValue(100);

                for (int i = 0; i < 100; i++)
                {
                    task.Increment(1);
                    await Task.Delay(10);
                }
            }).ConfigureAwait(false);
        });
        await logger.FlushAsync();

        // Assert
        testConsole.Output.Should().Contain("Processing...");
    }
}
