using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels;

internal class DesignShellViewModel : IShellViewModel
{
    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string> { "test" };

    public CollectionViewSource Parameters { get; init; } = new CollectionViewSource();

    public ObservableCollection<Parameter> Results { get; init; } = new ObservableCollection<Parameter>();

    public string? SelectedMachineId { get; set; } = "test";

    public Task InitializeAsync()
    {
        return Task.FromResult<Task>(null!);
    }

    public void LogException(string exceptionMessage)
    {
    }
}