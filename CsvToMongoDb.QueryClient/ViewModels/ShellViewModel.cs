using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ShellViewModel : ObservableObject, IShellViewModel
{
    private readonly ISearchService _searchService;
    private string? _selectedMachineId;

    private ObservableCollection<SearchResult>? _type;
    private ObservableCollection<Parameter> _results = new ObservableCollection<Parameter>();
    private string _parameterFilter;
    private IList<ParameterViewModel> _parameters = new List<ParameterViewModel>();
    private CollectionViewSource _parametersViewSource = new CollectionViewSource();

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>();

    public CollectionViewSource Parameters
    {
        get => _parametersViewSource;
        set
        {
            if (Equals(value, _parametersViewSource))
            {
                return;
            }

            _parametersViewSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Parameter> Results
    {
        get => _results;
        set
        {
            if (Equals(value, _results))
            {
                return;
            }

            _results = value;
            OnPropertyChanged();
        }
    }

    public string? SelectedMachineId
    {
        get => _selectedMachineId;
        set
        {
            if (value == _selectedMachineId)
            {
                return;
            }

            _selectedMachineId = value;
            OnPropertyChanged();
        }
    }

    public string ParameterFilter
    {
        get => _parameterFilter;
        set
        {
            if (value == _parameterFilter)
            {
                return;
            }

            _parameterFilter = value;
            OnPropertyChanged();
            if (Parameters != null)
            {
                Parameters.View?.Refresh();
            }
        }
    }

    public ShellViewModel(ISearchService searchService)
    {
        _searchService = searchService;
    }

    private void FilterParameters(object obj, FilterEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ParameterFilter))
        {
            e.Accepted = true;
            return;
        }
        else if (e.Item is ParameterViewModel parameterViewModel)
        {
            
            e.Accepted = parameterViewModel.Name.Contains(ParameterFilter, StringComparison.OrdinalIgnoreCase);
            return;
        }

        e.Accepted = false;
    }

    public async Task InitializeAsync()
    {
        var allMachineIdsAsync = await _searchService.GetAllMachineIdsAsync().ConfigureAwait(true);
        allMachineIdsAsync.OrderBy(s => s).ToList().ForEach(m => MachineIds.Add(m));
        SelectedMachineId = MachineIds.FirstOrDefault();
        var allParametersAsync = await _searchService.GetAllParametersByMachineIdAsync(_selectedMachineId).ConfigureAwait(true);
        foreach (var p in allParametersAsync.ToList())
        {
            var parameterViewModel = new ParameterViewModel(p);
            parameterViewModel.OnIsSelectedChanged += async (_, _) => await Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
            _parameters.Add(parameterViewModel);
        }
        Parameters = new CollectionViewSource { Source = _parameters };
        Parameters.Filter += FilterParameters;
        Parameters.View.Refresh();
    }

    private async Task SearchResultsAsync()
    {
        var results = await _searchService.SearchEverywhereAsync(new[] { SelectedMachineId }, _parameters.Where(p => p.IsSelected).Select(p => p.Name).ToArray()).ConfigureAwait(true);

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