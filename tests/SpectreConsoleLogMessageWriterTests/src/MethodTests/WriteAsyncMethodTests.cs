using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace SpectreConsoleLogMessageWriterTests.MethodTests.WriteAsyncMethodTests;

internal sealed class LogMessage<TPayload> : ILogMessage<TPayload>
    where TPayload : notnull
{
    public required TPayload Payload { get; init; }

    public required DateTimeOffset Timestamp { get; init; }

    public required IReadOnlyList<string> Senders { get; init; }

    public required LogLevel? LogLevel { get; init; }

    object ILogMessage.Payload => Payload;
}

public sealed class TheWriteAsyncMethod
{
    [Test]
    [Arguments(null)]
    [Arguments(LogLevel.Info)]
    [Arguments(LogLevel.Warning)]
    [Arguments(LogLevel.Error)]
    public async Task ShouldWriteLogMessagesWithDifferentLogLevels(LogLevel? logLevel)
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        SpectreConsoleLogSink logSink = new()
        {
            Console = testConsole
        };
        SpectreConsoleLogMessageWriter<object> logMessageWriter = new(logSink);

        // Act
        await logMessageWriter.WriteAsync(new LogMessage<object>
        {
            Timestamp = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            LogLevel = logLevel,
            Senders = [],
            Payload = "Payload"
        }, default);

        // Assert
        await Verifier.Verify(testConsole.Output);
    }
    [Test]
    [Arguments()]
    [Arguments("Sender1")]
    [Arguments("Sender1", "Sender2")]
    public async Task ShouldWriteLogMessagesWithDifferentSenders(params string[] senders)
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        testConsole.Profile.Width = 120;
        SpectreConsoleLogSink logSink = new()
        {
            Console = testConsole
        };
        SpectreConsoleLogMessageWriter<object> logMessageWriter = new(logSink);

        // Act
        await logMessageWriter.WriteAsync(new LogMessage<object>
        {
            Timestamp = new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
            LogLevel = null,
            Senders = senders,
            Payload = "Payload"
        }, default);

        // Assert
        await Verifier.Verify(testConsole.Output).UseParameters(senders);
    }
}
