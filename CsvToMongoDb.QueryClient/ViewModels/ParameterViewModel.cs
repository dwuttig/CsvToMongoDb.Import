using CommunityToolkit.Mvvm.ComponentModel;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ParameterViewModel : ObservableObject, IParameterViewModel
{
    public bool IsSelected { get; set; }

    public string Name { get; set; }
}
