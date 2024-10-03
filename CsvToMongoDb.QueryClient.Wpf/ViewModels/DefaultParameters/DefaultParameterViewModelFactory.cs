using CsvToMongoDb.QueryClient.Wpf.Infrastructure;
using CsvToMongoDb.QueryClient.Wpf.Services;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public class DefaultParameterViewModelFactory : IDefaultParameterViewModelFactory
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly IEventAggregator _eventAggregator;

    public DefaultParameterViewModelFactory(IUserSettingsService userSettingsService, IEventAggregator eventAggregator)
    {
        _userSettingsService = userSettingsService;
        _eventAggregator = eventAggregator;
    }

    public IDefaultParameterViewModel Create(string key, string name)
    {
        return new DefaultParameterViewModel(key, name, _userSettingsService, _eventAggregator);
    }
}