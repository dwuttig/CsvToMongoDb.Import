using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.Wpf.Configuration;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public sealed class DefaultParametersViewModel : ObservableObject, IDefaultParametersViewModel
{
    public DefaultParametersViewModel(IDefaultParameterReader defaultParameterReader, IDefaultParameterViewModelFactory defaultParameterViewModelFactory)
    {
        var defaultParameters = defaultParameterReader.GetDefaultParameters();
        foreach (var defaultParameterGroup in defaultParameters.GetAllDefaultParameterGroups())
        {
            var defaultParameterGroupViewModel = new DefaultParameterGroupViewModel(defaultParameterGroup);
            foreach (var defaultParameter in defaultParameters.GetAllDefaultParameters(defaultParameterGroup))
            {
                var defaultParameterViewModel = defaultParameterViewModelFactory.Create(
                    defaultParameter,
                    defaultParameters.GetDefaultParameterName(defaultParameterGroup, defaultParameter));

                defaultParameterGroupViewModel.Parameters.Add(defaultParameterViewModel);
            }

            DefaultParameterGroups.Add(defaultParameterGroupViewModel);
        }
    }

    public ObservableCollection<DefaultParameterGroupViewModel> DefaultParameterGroups { get; } = new ObservableCollection<DefaultParameterGroupViewModel>();

    public IList<string> GetSelectedDefaultParameters()
    {
        return DefaultParameterGroups
            .SelectMany(dp => dp.Parameters)
            .Where(parameterViewModel => parameterViewModel.IsSelected)
            .Select(parameterViewModel => parameterViewModel.Key)
            .ToList();
    }
}