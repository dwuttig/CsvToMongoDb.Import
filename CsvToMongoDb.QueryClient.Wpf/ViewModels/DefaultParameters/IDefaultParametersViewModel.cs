using System.Collections.ObjectModel;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public interface IDefaultParametersViewModel
{
    ObservableCollection<DefaultParameterGroupViewModel> DefaultParameterGroups { get; }

    IList<string> GetSelectedDefaultParameters();
}