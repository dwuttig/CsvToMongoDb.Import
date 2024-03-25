using System.Collections.ObjectModel;
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

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>();

    public ObservableCollection<ParameterViewModel> Parameters { get; set; } = new ObservableCollection<ParameterViewModel>();

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

    public ShellViewModel(ISearchService searchService)
    {
        _searchService = searchService;
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
            Parameters.Add(parameterViewModel);
        }
    }

    private async Task SearchResultsAsync()
    {
        var results = await _searchService.SearchEverywhereAsync(new[] { SelectedMachineId }, Parameters.Where(p => p.IsSelected).Select(p => p.Name).ToArray()).ConfigureAwait(true);

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