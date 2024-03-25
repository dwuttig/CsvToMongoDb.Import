using System.Collections.ObjectModel;

namespace CsvToMongoDb.QueryClient.ViewModels;

public interface IShellViewModel
{
    Task InitializeAsync();

    ObservableCollection<ParameterViewModel> Parameters { get; set; }

    ObservableCollection<string> MachineIds { get; set; }

    string? SelectedMachineId { get; set; }
}