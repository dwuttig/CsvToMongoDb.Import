using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.ViewModels;

internal class DesignShellViewModel : IShellViewModel
{
    public Task InitializeAsync()
    {
        return Task.FromResult<Task>(null!);
    }

    public CollectionViewSource Parameters { get; init; } = new CollectionViewSource();

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>() { "test" };

    public string? SelectedMachineId { get; set; } = "test";

    public ObservableCollection<Parameter> Results { get; init; } = new ObservableCollection<Parameter>();

    public void LogException(string exceptionMessage)
    {
    }
}