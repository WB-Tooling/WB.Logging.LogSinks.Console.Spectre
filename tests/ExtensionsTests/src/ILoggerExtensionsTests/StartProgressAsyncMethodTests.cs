using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.StartProgressAsyncMethodTests;

public sealed class TheStartProgressAsyncMethod
{
    [Test]
    public async Task ShouldStartAProgressTaskAndLogItToTheConsole()
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true,
        };
        ILogger logger = new Logger("Test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Writer = testConsole;
        });

        // Act
        await logger.StartProgressAsync("Processing...", async progress =>
        {
            var task = progress.AddTask("Processing files", maxValue: 100);

            while (!progress.IsFinished)
            {
                task.Increment(1);

                await Task.Delay(100).ConfigureAwait(false);
            }
        }).ConfigureAwait(false);
        await logger.FlushAsync().ConfigureAwait(false);

        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }
}
