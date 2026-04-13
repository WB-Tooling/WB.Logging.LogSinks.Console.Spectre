namespace ExtensionsTests.ObjectExtensionsTests.HasOverriddenToStringMethodTests;

using System;
using AwesomeAssertions;
using WB.Logging.LogSinks.Console.Spectre;

internal sealed class TypeWithOverriddenToStringMethod
{
    public override string ToString()
    {
        return "Overridden ToString method";
    }
}

internal sealed class TypeWithoutOverriddenToStringMethod
{
}

public sealed class TheHasOverriddenToStringMethod
{
    [Test]
    public void ShouldReturnTrueForTypeWithOverriddenToStringMethod()
    {
        // Arrange
        TypeWithOverriddenToStringMethod instance = new();

        // Act
        bool result = instance.HasOverriddenToString();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void ShouldReturnFalseForTypeWithoutOverriddenToStringMethod()
    {
        // Arrange
        TypeWithoutOverriddenToStringMethod instance = new();

        // Act
        bool result = instance.HasOverriddenToString();

        // Assert
        result.Should().BeFalse();
    }
}