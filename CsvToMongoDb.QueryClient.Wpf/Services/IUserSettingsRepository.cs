using CsvToMongoDb.QueryClient.Wpf.Models;

namespace CsvToMongoDb.QueryClient.Wpf.Services;

public interface IUserSettingsRepository
{
    void SaveUserSettings(UserSettings userSettings);

    UserSettings GetUserSettings();
}