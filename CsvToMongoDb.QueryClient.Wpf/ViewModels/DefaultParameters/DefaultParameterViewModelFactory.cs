using CsvToMongoDb.QueryClient.Wpf.Services;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public class DefaultParameterViewModelFactory : IDefaultParameterViewModelFactory
{
    private readonly IUserSettingsService _userSettingsService;

    public DefaultParameterViewModelFactory(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
    }

    public DefaultParameterViewModel Create(string key, string name)
    {
        return new DefaultParameterViewModel(key, name, _userSettingsService);
    }
}