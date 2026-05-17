using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace WidgetTests.LogMessageWidgetTests.MethodTests.RenderMethodTests;

public record TestData(DateTimeOffset? Timestamp, LogLevel? LogLevel, IReadOnlyList<string>? Senders, object? Payload, string TestName);

public static class TestDataProvider
{
    public static Func<TestData> AllNull()
        => () => new TestData(null, null, null, null, "AllNull");

    public static Func<TestData> TimestampOnly()
        => () => new TestData(DateTimeOffset.MinValue, null, null, null, "TimestampOnly");

    public static Func<TestData> LogLevelOnly()
        => () => new TestData(null, LogLevel.Info, null, null, "LogLevelOnly");

    public static Func<TestData> SendersOnly()
        => () => new TestData(null, null, ["Sender1", "Sender2"], null, "SendersOnly");

    public static Func<TestData> PayloadOnly()
        => () => new TestData(null, null, null, "Payload", "PayloadOnly");

    public static Func<TestData> AVeryLongPayload()
        => () => new TestData(null, null, null, new string('A', 80 * 2), "AVeryLongPayload");

    public static Func<TestData> AllNotNull()
        => () => new TestData(DateTimeOffset.MinValue, LogLevel.Info, ["Sender1"], "Payload", "AllNotNull");

    public static Func<TestData> AllNotNullWithTooLongBreakablePayload()
        => () => new TestData(DateTimeOffset.MinValue, LogLevel.Info, ["Sender1"], string.Concat(Enumerable.Repeat("A ", 40)), "AllNotNullWithTooLongBreakablePayload");

    public static Func<TestData> AllNotNullWithTooLongNonBreakablePayload()
        => () => new TestData(DateTimeOffset.MinValue, LogLevel.Info, ["Sender1"], new string('A', 80 * 2), "AllNotNullWithTooLongNonBreakablePayload");
}

public sealed class TheRenderMethod
{
    [Test]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AllNull))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.TimestampOnly))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.LogLevelOnly))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.SendersOnly))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.PayloadOnly))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AllNotNull))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AVeryLongPayload))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AllNotNullWithTooLongBreakablePayload))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AllNotNullWithTooLongNonBreakablePayload))]
    public async Task ShouldRenderTheLogMessageWidget(TestData testData)
    {
        // Arrange
        using TestConsole testConsole = new()
        {
            EmitAnsiSequences = true,
        };

        LogMessageWidget logMessageWidget = new()
        {
            Timestamp = testData.Timestamp is null ? [] : [testData.Timestamp.ToString().ToMarkup()],
            LogLevel = testData.LogLevel is null ? [] : [testData.LogLevel.ToString().ToMarkup()],
            Senders = testData.Senders is null ? [] : [string.Join(", ", testData.Senders).ToMarkup()],
            Payload = testData.Payload is null ? [] : [testData.Payload.ToString().ToMarkup()]
        };

        // Act
        testConsole.Write(logMessageWidget);

        // Assert
        await Verifier.Verify(testConsole.Output).UseFileName(testData.TestName).ConfigureAwait(false);
    }
}
