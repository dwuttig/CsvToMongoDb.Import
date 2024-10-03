using System.IO;
using System.Xml.Serialization;
using CsvToMongoDb.QueryClient.Wpf.Models;

namespace CsvToMongoDb.QueryClient.Wpf.Services;

public class UserSettingsFileRepository : IUserSettingsRepository
{
    private readonly string _filePath;

    public UserSettingsFileRepository(string filePath)
    {
        _filePath = filePath;
    }

    public UserSettingsFileRepository()
    {
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _filePath = Path.Combine(appDataFolder, "usersettings.config");
    }

    public void SaveUserSettings(UserSettings userSettings)
    {
        var serializer = new XmlSerializer(typeof(UserSettings));
        using var writer = new StreamWriter(_filePath);
        serializer.Serialize(writer, userSettings);
    }

    public UserSettings GetUserSettings()
    {
        if (!File.Exists(_filePath))
        {
            return new UserSettings();
        }

        var serializer = new XmlSerializer(typeof(UserSettings));
        using var reader = new StreamReader(_filePath);
        var userSettings = (UserSettings)serializer.Deserialize(reader);
        return userSettings;
    }
}