using System.Collections.ObjectModel;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DesignDefaultParametersViewModel : IDefaultParametersViewModel
{
    public ObservableCollection<DefaultParameterGroupViewModel> DefaultParameterGroups { get; } = new();

    public IList<string> GetSelectedDefaultParameters()
    {
        throw new NotImplementedException();
    }

    public DesignDefaultParametersViewModel()
    {
        var defaultParameterGroupViewModel = new DefaultParameterGroupViewModel("Test");
        defaultParameterGroupViewModel.Parameters.Add(new DesignDefaultParameterViewModel("Parameter_Key"));
        DefaultParameterGroups.Add(defaultParameterGroupViewModel);
    }
}