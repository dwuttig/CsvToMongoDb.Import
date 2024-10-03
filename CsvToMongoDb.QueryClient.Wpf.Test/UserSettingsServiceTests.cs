using CsvToMongoDb.QueryClient.Wpf.Models;
using CsvToMongoDb.QueryClient.Wpf.Services;
using Shouldly;

namespace CsvToMongoDb.QueryClient.Wpf.Test;

[TestFixture]
public class UserSettingsServiceTests
{
    private const string SettingsXml = "./settings.xml";
    private readonly UserSettingsFileRepository _userSettingsFileRepository = new UserSettingsFileRepository(SettingsXml);
    
    
    [TearDown]
    public void DeleteSettingsFile()
    {
        if (File.Exists(SettingsXml))
        {
            File.Delete(SettingsXml);
        }
    }

    [Test]
    public void UpdateUserSettings_CallsUpdateActionWithUserSettings()
    {
        // Arrange
        Action<UserSettings> updateAction = (settings) => settings.AddOrUpdateDefaultParametersSelection("key", true);
        var userSettingsService = new UserSettingsService(_userSettingsFileRepository);

        // Act
        userSettingsService.UpdateUserSettings(updateAction);
        userSettingsService.GetUserSettings().GetDefaultParametersSelection().TryGetValue("key", out var isSelected);

        // Assert
        isSelected.ShouldBeTrue();
    }
}