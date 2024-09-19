using CommunityToolkit.Mvvm.ComponentModel;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DefaultParameterViewModel : ObservableObject
{
    private readonly string _key;
    private bool _isSelected;

    public string Name { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public DefaultParameterViewModel(string key,string name)
    {
        _key = key;
        Name = name;
    }
}