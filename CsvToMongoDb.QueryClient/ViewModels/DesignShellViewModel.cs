using System.Collections.ObjectModel;

namespace CsvToMongoDb.QueryClient.ViewModels;

internal class DesignShellViewModel : IShellViewModel
{
    public Task InitializeAsync()
    {
        return Task.FromResult<>(null);
    }

    public ObservableCollection<ParameterViewModel> Parameters { get; set; } = new ObservableCollection<ParameterViewModel>() { new ParameterViewModel("Test") };

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>() { "test" };

    public string? SelectedMachineId { get; set; } = "test";
}