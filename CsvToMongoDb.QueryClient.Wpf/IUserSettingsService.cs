namespace CsvToMongoDb.QueryClient.Wpf;

public interface IUserSettingsService
{
    void SaveUserSettings(UserSettings userSettings);

    UserSettings LoadUserSettings();
}