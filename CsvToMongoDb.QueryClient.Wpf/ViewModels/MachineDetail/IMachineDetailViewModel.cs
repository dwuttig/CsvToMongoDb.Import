using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;

public interface IMachineDetailViewModel
{
    ObservableCollection<string> MachineIds { get; set; }

    CollectionViewSource Parameters { get; }

    ObservableCollection<Parameter> Results { get; }

    string? SelectedMachineId { get; set; }

    CollectionViewSource SelectedParameters { get; }
    
    RelayCommand DeselectAllCommand { get; }

    Task InitializeAsync();

    void LogException(string exceptionMessage);
}