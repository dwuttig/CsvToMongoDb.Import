using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.ViewModels;

public interface IShellViewModel
{
    Task InitializeAsync();

    CollectionViewSource Parameters { get; }

    ObservableCollection<string> MachineIds { get; set; }

    string? SelectedMachineId { get; set; }

    ObservableCollection<Parameter> Results { get; init; }

    void LogException(string exceptionMessage);
}