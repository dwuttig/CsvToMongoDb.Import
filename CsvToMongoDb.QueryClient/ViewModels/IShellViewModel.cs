using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CsvToMongoDb.QueryClient.ViewModels;

public interface IShellViewModel
{
    Task InitializeAsync();

    CollectionViewSource Parameters { get; set; }

    ObservableCollection<string> MachineIds { get; set; }

    string? SelectedMachineId { get; set; }
}