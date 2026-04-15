using System;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging.LogSinks.Console.Spectre;

namespace WidgetTests.BadgeWidgetTests.MethodTests.RenderMethodTests;

public record TestData(Style BracketStyle, Style TextStyle);

public static class TestDataProvider
{
    public static Func<TestData> AllPlain()
        => () => new TestData(Style.Plain, Style.Plain);

    public static Func<TestData> BracketStyleRed()
        => () => new TestData(Style.Parse("red"), Style.Plain);

    public static Func<TestData> TextStyleGreen()
        => () => new TestData(Style.Plain, Style.Parse("green"));
}


public sealed class TheRenderMethod
{
    [Test]
    [Arguments("Test")]
    [Arguments("Another Test")]
    public Task ShouldRenderWithDifferentText(string text)
    {
        // Arrange
        BadgeWidget badgeWidget = new(text);

        // Act
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        testConsole.Write(badgeWidget);

        // Assert
        return Verifier.Verify(testConsole.Output);
    }

    [Test]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.AllPlain))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.BracketStyleRed))]
    [MethodDataSource(typeof(TestDataProvider), nameof(TestDataProvider.TextStyleGreen))]
    public Task ShouldRenderWithDifferentStyles(TestData testData)
    {
        // Arrange
        BadgeWidget badgeWidget = new("Test")
        {
            BracketStyle = testData.BracketStyle,
            TextStyle = testData.TextStyle
        };

        // Act
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };
        testConsole.Write(badgeWidget);

        // Assert
        return Verifier.Verify(testConsole.Output);
    }
}
