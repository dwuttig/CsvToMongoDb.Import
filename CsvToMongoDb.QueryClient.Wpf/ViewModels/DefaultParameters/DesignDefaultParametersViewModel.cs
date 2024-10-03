using System.Collections.ObjectModel;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DesignDefaultParametersViewModel : IDefaultParametersViewModel
{
    public ObservableCollection<DefaultParameterGroupViewModel> DefaultParameterGroups { get; } = new();

    public IList<string> GetSelectedDefaultParameters()
    {
        return Array.Empty<string>();
    }

    public DesignDefaultParametersViewModel()
    {
        var defaultParameterGroupViewModel = new DefaultParameterGroupViewModel("Test");
        defaultParameterGroupViewModel.Parameters.Add(new DesignDefaultParameterViewModel("Parameter_Key"));
        DefaultParameterGroups.Add(defaultParameterGroupViewModel);
    }
}