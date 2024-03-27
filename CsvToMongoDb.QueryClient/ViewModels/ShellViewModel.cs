using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;
using Microsoft.Extensions.Configuration;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ShellViewModel : ObservableObject, IShellViewModel
{
    private CollectionViewSource _parametersViewSource = new CollectionViewSource();
    private IList<ParameterViewModel> _parameters = new List<ParameterViewModel>();
    private ObservableCollection<Parameter> _results = new ObservableCollection<Parameter>();
    private readonly ISearchService _searchService;
    private readonly IImportService _importService;
    private string? _parameterFilter;
    private string? _selectedMachineId;
    private readonly string? _archivePath;
    private string? _watchPath;
    private StringBuilder _importLogBuilder = new StringBuilder();

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>();

    public CollectionViewSource Parameters
    {
        get => _parametersViewSource;
        init => _parametersViewSource = value;
    }

    public ObservableCollection<Parameter> Results
    {
        get => _results;
        init => _results = value;
    }

    public string? SelectedMachineId
    {
        get => _selectedMachineId;
        set
        {
            if (SetProperty(ref _selectedMachineId, value))
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

    public string ImportLog => _importLogBuilder.ToString();

    public ShellViewModel(ISearchService searchService, IImportService importService)
    {
        _searchService = searchService;
        _importService = importService;
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();
        _archivePath = configuration["ArchivePath"];
        _watchPath = configuration["WatchPath"];
        Parameters = new CollectionViewSource { Source = _parameters };
        Results = new ObservableCollection<Parameter>();
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

        Parameters.Filter += FilterParameters;
        Parameters.View.Refresh();
        foreach (var file in Directory.GetFiles(_watchPath, "*.csv"))
        {
            _importLogBuilder.AppendLine($"Importing {file}.");
            OnPropertyChanged(nameof(ImportLog));
            var fileName = string.Empty;
            await Task.Run(() => { fileName = ImportFile(file); });
            _importLogBuilder.AppendLine($"{fileName} imported.");
            OnPropertyChanged(nameof(ImportLog));
        }
    }

    private string ImportFile(string file)
    {
        var fileInfo = new FileInfo(file);
        _importService.ImportCsvData(fileInfo.FullName);
        File.Move(fileInfo.FullName, Path.Combine(_archivePath, fileInfo.Name), true);
        return fileInfo.Name;
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