using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CsvToMongoDb.QueryClient.ViewModels;

internal class DesignShellViewModel : IShellViewModel
{
    public Task InitializeAsync()
    {
        return Task.FromResult<Task>(null!);
    }

    public CollectionViewSource Parameters { get; set; } = new CollectionViewSource();

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>() { "test" };

    public string? SelectedMachineId { get; set; } = "test";
}