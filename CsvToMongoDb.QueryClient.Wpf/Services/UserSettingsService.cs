using System.IO;
using System.Xml.Serialization;
using CsvToMongoDb.QueryClient.Wpf.Models;

namespace CsvToMongoDb.QueryClient.Wpf.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IUserSettingsRepository _userSettingsRepository;

    public UserSettingsService(IUserSettingsRepository userSettingsRepository)
    {
        _userSettingsRepository = userSettingsRepository;
    }

    public UserSettings GetUserSettings()
    {
        return _userSettingsRepository.GetUserSettings();
    }

    public void UpdateUserSettings(Action<UserSettings> updateAction)
    {
        var userSettings = _userSettingsRepository.GetUserSettings();
        updateAction(userSettings);
        _userSettingsRepository.SaveUserSettings(userSettings);
    }
}