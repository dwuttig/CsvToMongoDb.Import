using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

public class DesignParameterSearchViewModel : IParameterSearchViewModel
{
    public bool IsSoftStarter { get; set; }

    public bool IsDrive { get; set; }

    public bool IsGtStarter { get; set; }

    public CollectionViewSource Parameters { get; init; } = new CollectionViewSource();

    public string? ParameterFilter { get; set; }
    
    public ObservableCollection<Parameter> Results { get; init; } = new ObservableCollection<Parameter>();

    public DesignParameterSearchViewModel()
    {
        new AsyncRelayCommand(Search);
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

    private static Task Search()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}