using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DefaultParameterGroupViewModel : ObservableObject
{
    public DefaultParameterGroupViewModel(string name)
    {
        Name = name;
        Parameters = new ObservableCollection<IDefaultParameterViewModel>();
    }

    public string Name { get; }

    public ObservableCollection<IDefaultParameterViewModel> Parameters { get; }
}