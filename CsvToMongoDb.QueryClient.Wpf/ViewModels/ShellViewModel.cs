using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.Wpf.Configuration;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels;

public class ShellViewModel : ObservableObject, IShellViewModel
{
    public IMachineDetailViewModel MachineDetailViewModel { get; }

    public IParameterSearchViewModel ParameterSearchViewModel { get; }

    public IDefaultParametersViewModel DefaultParametersViewModel { get; }

    public ShellViewModel(
        IMachineDetailViewModel machineDetailViewModel,
        IParameterSearchViewModel parameterSearchViewModel,
        IDefaultParametersViewModel defaultParametersViewModel)
    {
        MachineDetailViewModel = machineDetailViewModel;
        ParameterSearchViewModel = parameterSearchViewModel;
        DefaultParametersViewModel = defaultParametersViewModel;
    }
}