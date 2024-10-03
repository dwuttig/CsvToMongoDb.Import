using CsvToMongoDb.QueryClient.Wpf.Models;

namespace CsvToMongoDb.QueryClient.Wpf.Services;

public interface IUserSettingsService
{
    UserSettings GetUserSettings();
    
    void UpdateUserSettings(Action<UserSettings> updateAction);
}