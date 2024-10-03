using NUnit.Framework;
using System.Collections.Generic;
using CsvToMongoDb.QueryClient.Wpf.Models;

[TestFixture]
public class UserSettingsTests
{
    [Test]
    public void GetSelectionStateForParameter_KeyExists_ReturnsTrue()
    {
        // Arrange
        var userSettings = new UserSettings();
        userSettings.AddOrUpdateDefaultParametersSelection("key", true);

        // Act
        var result = userSettings.GetSelectionStateForParameter("key");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void GetSelectionStateForParameter_KeyDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var userSettings = new UserSettings();

        // Act
        var result = userSettings.GetSelectionStateForParameter("key");

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GetSelectionStateForParameter_KeyIsNull_ReturnsFalse()
    {
        // Arrange
        var userSettings = new UserSettings();

        // Act
        var result = userSettings.GetSelectionStateForParameter(null);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GetSelectionStateForParameter_KeyIsEmptyString_ReturnsFalse()
    {
        // Arrange
        var userSettings = new UserSettings();

        // Act
        var result = userSettings.GetSelectionStateForParameter("");

        // Assert
        Assert.IsFalse(result);
    }
}