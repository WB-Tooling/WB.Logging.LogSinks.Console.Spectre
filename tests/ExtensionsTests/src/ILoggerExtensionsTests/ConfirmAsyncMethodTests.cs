using System.Threading.Tasks;
using AwesomeAssertions;
using Spectre.Console.Testing;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.ConfirmAsyncMethodTests;

public sealed class TheConfirmAsyncMethod
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task ShouldAskAQuestion(bool answer)
    {
        // Arrange
        string input = answer ? "y" : "n";
        TestConsole testConsole = new();
        testConsole.Input.PushTextWithEnter(input);
        Logger logger = new("test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Console = testConsole;
        });

        // Act
        bool result = await logger.ConfirmAsync("What is the answer to the Ultimate Question of Life, The Universe, and Everything?").ConfigureAwait(false);
        await logger.FlushAsync().ConfigureAwait(false);

        // Assert
        result.Should().Be(answer, because: "the ConfirmAsync method should return the input provided by the user");
    }
}
