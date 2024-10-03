using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.QueryClient.Wpf.Infrastructure;
using CsvToMongoDb.QueryClient.Wpf.Services;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DefaultParameterViewModel : ObservableObject, IDefaultParameterViewModel
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly IEventAggregator _eventAggregator;
    private bool _isSelected;

    public string Key { get; }

    public string Name { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value))
            {
                _userSettingsService.UpdateUserSettings(settings => settings.AddOrUpdateDefaultParametersSelection(Key, value));
                _eventAggregator.Publish(DefaultParameterSelectionChangedEvent.Default);
            }
        }
    }

    public DefaultParameterViewModel(string key, string name, IUserSettingsService userSettingsService, IEventAggregator eventAggregator)
    {
        Key = key;
        Name = name;
        _userSettingsService = userSettingsService;
        _eventAggregator = eventAggregator;
        _isSelected = _userSettingsService.GetUserSettings().GetSelectionStateForParameter(Key);
    }
}