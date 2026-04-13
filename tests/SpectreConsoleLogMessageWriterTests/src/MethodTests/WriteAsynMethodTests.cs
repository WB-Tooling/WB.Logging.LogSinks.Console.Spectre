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
    public Task ShouldWriteLogMessageWithAllParts()
    {
        // Arrange
        SpectreConsoleLogMessageWriter<object> logMessageWriter = new();

        // Act
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        logMessageWriter.AnsiConsole = testConsole;
        logMessageWriter.WriteAsync(
            timestamp: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            logLevel: LogLevel.Warning,
            senders: ["Sender1", "Sender2"],
            payload: "0123456789 0123456789 0123456789 0123456789 0123456789 0123456789 0123456789 0123456789 0123456789 0123456789");

        // Assert
        return Verifier.Verify(testConsole.Output);
    }
}