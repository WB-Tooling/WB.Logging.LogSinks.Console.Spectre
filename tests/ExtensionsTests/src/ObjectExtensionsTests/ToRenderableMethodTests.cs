using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyTUnit;
using WB.Logging.LogSinks.Console.Spectre;

namespace ExtensionsTests.ObjectExtensionsTests.ToRenderableMethodTests;

internal sealed class TypeWithOverriddenToStringMethod
{
    public override string ToString()
    {
        return "Overridden ToString method";
    }
}

internal sealed class CustomObject
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
}

public sealed class TheToRenderableMethod
{
    [Test]
    public async Task ShouldReturnTextForNull()
    {
        // Arrange
        object? input = null;
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToRenderable());

        // Assert
        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }

    [Test]
    [Arguments("Hello, World!")]
    [Arguments("[red]Hello, World![/]")]
    public async Task ShouldRenderAString(string @string)
    {
        // Arrange
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(@string.ToRenderable());

        // Assert
        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }

    [Test]
    public async Task ShouldRenderObjectWithOverriddenToStringMethod()
    {
        // Arrange
        TypeWithOverriddenToStringMethod input = new();
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToRenderable());

        // Assert
        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }

    [Test]
    public async Task ShouldRenderCustomObjectAsJson()
    {
        // Arrange
        CustomObject input = new()
        {
            Name = "Test Object",
            Value = 42
        };
        TestConsole testConsole = new()
        {
            EmitAnsiSequences = true
        };

        // Act
        testConsole.Write(input.ToRenderable());

        // Assert
        await Verifier.Verify(testConsole.Output).ConfigureAwait(false);
    }
}
