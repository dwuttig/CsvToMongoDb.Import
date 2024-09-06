using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;

public class DesignMachineDetailViewModel : IMachineDetailViewModel
{
    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>();

    public CollectionViewSource Parameters { get; } = new CollectionViewSource();

    public ObservableCollection<Parameter> Results { get; init; } = new ObservableCollection<Parameter>();

    public string? SelectedMachineId { get; set; } = "MachineId1";

    public DesignMachineDetailViewModel()
    {
        MachineIds.Add("MachineId1");
        MachineIds.Add("MachineId2");

        Parameters.Source = new List<ParameterViewModel>()
        {
            new ParameterViewModel("Parameter1") { IsSelected = true },
            new ParameterViewModel("Parameter2"),
            new ParameterViewModel("Parameter3") { IsSelected = true },
            new ParameterViewModel("Parameter4"),
            new ParameterViewModel("Parameter5"),
        };
        
        Results.Add(new Parameter("Collection", "Parameter1", "Parameter1", "Value1", "Unit1"));
        Results.Add(new Parameter("Collection", "Parameter3", "Parameter3", "Value3", "Unit3"));
    }

    public Task InitializeAsync()
    {
       return Task.CompletedTask;
    }

    public void LogException(string exceptionMessage)
    {
    }
}