namespace CsvToMongoDb.QueryClient;

public interface IUserSettingsService
{
    void SaveUserSettings(UserSettings userSettings);

    UserSettings LoadUserSettings();
}