using System;
using AwesomeAssertions;
using WB.Logging;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ILoggerExtensionsTests.AttacheSpectreConsoleMethodTests;

public sealed class TheAttachSpectreConsoleMethod
{
    [Test]
    public void ShouldAttachANewSpectreConsoleLogSinkToTheLogger()
    {
        // Arrange
        Logger logger = new("Test");

        // Act
        using IDisposable disposable = logger.AttachSpectreConsole();

        // Assert
        logger.AsyncLogSinks.Should()
            .ContainSingle(because: "the AttachSpectreConsole method should attach exactly one log sink to the logger")
            .Which.Should()
            .BeOfType<SpectreConsoleLogSink>(because: "the AttachSpectreConsole method should attach a new SpectreConsoleLogSink to the logger");
    }

    [Test]
    public void ShouldAcceptAnActionThatProvidesAccessToTheSpectreConsoleLogSink()
    {
        // Arrange
        Logger logger = new("Test");
        SpectreConsoleLogSink? capturedLogSink = null;

        // Act
        using IDisposable disposable = logger.AttachSpectreConsole(logSink =>
        {
            capturedLogSink = logSink;
        });

        // Assert
        capturedLogSink.Should().NotBeNull(because: "the configure action should be invoked with the created SpectreConsoleLogSink");
        logger.AsyncLogSinks.Should()
            .ContainSingle(because: "the AttachSpectreConsole method should attach exactly one log sink to the logger")
            .Which.Should()
            .BeSameAs(capturedLogSink, because: "the SpectreConsoleLogSink passed to the configure action should be the same instance that was attached to the logger");

    }
}
