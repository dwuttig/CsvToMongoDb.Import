using System.IO;
using System.Xml.Serialization;

namespace CsvToMongoDb.QueryClient.Wpf;

public class UserSettingsService : IUserSettingsService
{
    private readonly string _filePath;

    public UserSettingsService()
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

    public UserSettings LoadUserSettings()
    {
        if (!File.Exists(_filePath))
        {
            return new UserSettings();
        }

        var serializer = new XmlSerializer(typeof(UserSettings));
        using var reader = new StreamReader(_filePath);
        return (UserSettings)serializer.Deserialize(reader);
    }
}