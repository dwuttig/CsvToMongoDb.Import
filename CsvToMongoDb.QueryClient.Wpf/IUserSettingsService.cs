namespace CsvToMongoDb.QueryClient.Wpf;

public interface IUserSettingsService
{
    UserSettings LoadUserSettings();

    void SaveUserSettings(UserSettings userSettings);
}