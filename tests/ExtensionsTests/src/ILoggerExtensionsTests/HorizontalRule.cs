using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;

namespace WB.Logging.LogSinks.Console.Spectre.Extensions;

public sealed class TheHorizontalRuleMethod
{
    [Test]
    [Arguments(null)]
    [Arguments("My Title")]
    public async Task ShouldLogAHorizontalRuleToTheConsole(string? title)
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true,
        };
        Logger logger = new("Test");
        logger.AttachSpectreConsole(logSink =>
        {
            logSink.Writer = testConsole;
        });

        // Act
        logger.HorizontalRule(title);
        await logger.FlushAsync().ConfigureAwait(false);

        // Assert
        await Verifier.Verify(testConsole.Output).UseParameters(title).ConfigureAwait(false);
    }
}