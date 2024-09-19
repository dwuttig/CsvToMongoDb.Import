using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels;

public interface IShellViewModel
{
    IMachineDetailViewModel MachineDetailViewModel { get; }

    IParameterSearchViewModel ParameterSearchViewModel { get; }

    IDefaultParametersViewModel DefaultParametersViewModel { get; }
}