using System;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
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
        await logger.StartProgressAsync(async (progress, cancellationToken) =>
        {
            var task = progress.AddTask("Processing files", maxValue: 100);

            while (!progress.IsFinished)
            {
                task.Increment(1);

                await Task.Delay(10).ConfigureAwait(false);
            }
        }).ConfigureAwait(false);
        await logger.FlushAsync().ConfigureAwait(false);

        testConsole.Output.Should().NotBeEmpty(because: "a progress should have been logged to the console");
    }

    [Test]
    public async Task ShouldThrowTheExceptionFromProgressAction()
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
        Func<Task> action = () => logger.StartProgressAsync(async (progress, cancellationToken) =>
        {
            throw new Exception("Something went wrong during progress.");
        });

        // Assert
        await action.Should().ThrowAsync<Exception>(because: "the exception from the progress action should be propagated");
    }

    [Test]
    public async Task ShouldBeCancellable()
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

        using CancellationTokenSource cancellationTokenSource = new();

        // Act
        Func<Task> action = () => logger.StartProgressAsync(async (progress, cancellationToken) =>
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }, cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        // Assert
        await action.Should().ThrowAsync<OperationCanceledException>(because: "the progress should be canceled when the cancellation token is canceled");
    }
}
