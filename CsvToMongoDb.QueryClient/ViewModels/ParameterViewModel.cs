using CommunityToolkit.Mvvm.ComponentModel;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ParameterViewModel : ObservableObject, IParameterViewModel
{
    public EventHandler<EventArgs>? OnIsSelectedChanged;
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value != _isSelected)
            {
                _isSelected = value;
                OnIsSelectedChangedHandler(this, EventArgs.Empty);
            }
        }
    }

    public string Name { get; set; }

    public ParameterViewModel(string name)
    {
        Name = name;
    }

    public void OnIsSelectedChangedHandler(object sender, EventArgs e)
    {
        OnIsSelectedChanged?.Invoke(sender, e);
    }
}