using System.Threading.Tasks;
using AwesomeAssertions;
using Spectre.Console.Testing;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.AskAsyncMethodTests;

public sealed class TheAskAsyncMethod
{
    [Test]
    public async Task ShouldAskAQuestion()
    {
        // Arrange
        string input = "42";
        TestConsole testConsole = new();
        testConsole.Input.PushTextWithEnter(input);
        Logger logger = new("test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Console = testConsole;
        });

        // Act
        string result = await logger.AskAsync<string>("What is the answer to the Ultimate Question of Life, The Universe, and Everything?").ConfigureAwait(false);
        await logger.FlushAsync().ConfigureAwait(false);

        // Assert
        result.Should().Be(input, because: "the AskAsync method should return the input provided by the user");
    }
}
