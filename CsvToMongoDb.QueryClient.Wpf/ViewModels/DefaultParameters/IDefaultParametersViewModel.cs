using System.Collections.ObjectModel;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public interface IDefaultParametersViewModel
{
    ObservableCollection<DefaultParameterGroupViewModel> DefaultParameterGroups { get; }
}