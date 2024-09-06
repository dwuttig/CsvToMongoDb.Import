using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;

public interface IMachineDetailViewModel
{
    ObservableCollection<string> MachineIds { get; set; }

    CollectionViewSource Parameters { get; }

    ObservableCollection<Parameter> Results { get; init; }

    string? SelectedMachineId { get; set; }

    Task InitializeAsync();

    void LogException(string exceptionMessage);
}