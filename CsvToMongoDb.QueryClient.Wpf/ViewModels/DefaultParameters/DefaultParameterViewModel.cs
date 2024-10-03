using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.QueryClient.Wpf.Services;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DefaultParameterViewModel : ObservableObject, IDefaultParameterViewModel
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly string _key;
    private bool _isSelected;

    public string Name { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetProperty(ref _isSelected, value))
            {
                _userSettingsService.UpdateUserSettings(settings => settings.AddOrUpdateDefaultParametersSelection(_key, value));
            }
        }
    }

    public DefaultParameterViewModel(string key, string name, IUserSettingsService userSettingsService)
    {
        _key = key;
        Name = name;
        _userSettingsService = userSettingsService;
        _isSelected = _userSettingsService.GetUserSettings().GetSelectionStateForParameter(_key);
    }
}