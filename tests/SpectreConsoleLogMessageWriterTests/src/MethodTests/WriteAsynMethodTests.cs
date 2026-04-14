using System;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace SpectreConsoleLogMessageWriterTests.MethodTests.WriteAsynMethodTests;

public sealed class TheWriteAsyncMethod
{
    [Test]
    [Arguments(null)]
    [Arguments(LogLevel.Info)]
    [Arguments(LogLevel.Warning)]
    [Arguments(LogLevel.Error)]
    public Task ShouldWriteLogMessagesWithDifferentLogLevels(LogLevel? logLevel)
    {
        // Arrange
        SpectreConsoleLogMessageWriter<object> logMessageWriter = new();

        // Act
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        logMessageWriter.Console = testConsole;
        logMessageWriter.WriteAsync(
            timestamp: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            logLevel: logLevel,
            senders: ["Sender"],
                payload: "Payload");

        // Assert
        return Verifier.Verify(testConsole.Output);
    }
    [Test]
    [Arguments()]
    [Arguments("Sender1")]
    [Arguments("Sender1", "Sender2")]
    public Task ShouldWriteLogMessagesWithDifferentSenders(params string[] senders)
    {
        // Arrange
        SpectreConsoleLogMessageWriter<object> logMessageWriter = new();

        // Act
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        logMessageWriter.Console = testConsole;
        logMessageWriter.WriteAsync(
            timestamp: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            logLevel: null,
            senders: senders,
            payload: "Payload");

        // Assert
        return Verifier.Verify(testConsole.Output).UseParameters(senders);
    }
}