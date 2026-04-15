using System.Threading.Tasks;
using AwesomeAssertions;
using Spectre.Console;
using Spectre.Console.Testing;
using VerifyTUnit;

namespace WB.Logging.LogSinks.Console.Spectre;

public sealed class TheToMarkupMethod
{
    [Test]
    public Task ShouldReturnMarkup()
    {
        // Arrange
        string input = "[red]Hello, World![/]";
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToMarkup());

        // Assert
        return Verifier.Verify(testConsole.Output);
    }

    [Test]
    public Task ShouldReturnMarkupWithStyle()
    {
        // Arrange
        Style style = new Style(foreground: Color.Green, background: Color.Black, decoration: Decoration.Bold);
        string input = "[red]Hello, World![/]";
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToMarkup(style));

        // Assert
        return Verifier.Verify(testConsole.Output);
    }

    [Test]
    public Task ShouldRemoveBrokenMarkup()
    {
        // Arrange
        string input = "[red]Hello, World![]";
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToMarkup());

        // Assert
        return Verifier.Verify(testConsole.Output);
    }
}
