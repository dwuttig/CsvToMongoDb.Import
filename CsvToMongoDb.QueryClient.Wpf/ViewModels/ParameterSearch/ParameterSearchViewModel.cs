using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

public class ParameterSearchViewModel : ObservableObject, IParameterSearchViewModel
{
    private readonly ISearchService _searchService;
    private readonly IList<ParameterViewModel> _parameters = new List<ParameterViewModel>();
    private bool _isSoftStarter;
    private bool _isDrive;
    private bool _isGtStarter;
    private string? _parameterFilter;

    public bool IsDrive
    {
        get => _isDrive;
        set
        {
            if (SetProperty(ref _isDrive, value))
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
            }
        }
    }

    public bool IsGtStarter
    {
        get => _isGtStarter;
        set
        {
            if (SetProperty(ref _isGtStarter, value))

            {
                Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
            }
        }
    }

    public bool IsSoftStarter
    {
        get => _isSoftStarter;
        set
        {
            if (SetProperty(ref _isSoftStarter, value))
            {
                Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
            }
        }
    }

    public string? ParameterFilter
    {
        get => _parameterFilter;
        set
        {
            if (SetProperty(ref _parameterFilter, value))
            {
                Parameters.View?.Refresh();
            }
        }
    }

    public CollectionViewSource Parameters { get; init; }

    public ObservableCollection<Parameter> Results { get; init; }

    public ParameterSearchViewModel(ISearchService searchService)
    {
        _searchService = searchService;
        Parameters = new CollectionViewSource { Source = _parameters };
        Results = new ObservableCollection<Parameter>();
    }

    public async Task InitializeAsync()
    {
        var allParametersAsync = await _searchService.GetAllParameters().ConfigureAwait(true);

        foreach (var p in allParametersAsync.ToList())
        {
            var parameterViewModel = new ParameterViewModel(p);
            parameterViewModel.OnIsSelectedChanged += (_, _) => Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
            _parameters.Add(parameterViewModel);
        }

        Parameters.Filter += FilterParameters;
        Parameters.View.Refresh();
    }

    private void FilterParameters(object obj, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ParameterFilter))
        {
            e.Accepted = true;
            return;
        }

        if (e.Item is ParameterViewModel parameterViewModel)
        {
            e.Accepted = parameterViewModel.Name.Contains(ParameterFilter, StringComparison.OrdinalIgnoreCase);
            return;
        }

        e.Accepted = false;
    }

    private async Task SearchResultsAsync()
    {
        IList<MachineType> machineType = new List<MachineType>();
        if (IsSoftStarter)
        {
            machineType.Add(MachineType.SoftStarter);
        }

        if (IsDrive)
        {
            machineType.Add(MachineType.Drive);
        }

        if (IsGtStarter)
        {
            machineType.Add(MachineType.GTStarter);
        }

        var results = await _searchService.SearchByTypeAsync(machineType, _parameters.Where(p => p.IsSelected).Select(p => p.Name).ToArray());

        Results.Clear();
        foreach (var searchResult in results)
        {
            foreach (var parameter in searchResult.Parameters)
            {
                Results.Add(parameter);
            }
        }
    }
}