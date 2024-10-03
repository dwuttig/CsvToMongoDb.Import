using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.Wpf.Infrastructure;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;

public class MachineDetailViewModel : ObservableObject, IMachineDetailViewModel
{
    private readonly ISearchService _searchService;
    private readonly IImportService _importService;
    private readonly IDefaultParametersViewModel _defaultParametersViewModel;
    private readonly IList<ParameterViewModel> _parameters = new List<ParameterViewModel>();
    private readonly StringBuilder _importLogBuilder = new StringBuilder();
    private readonly PathConfiguration _pathConfiguration;
    private readonly IEventAggregator _eventAggregator;
    private readonly string fileMask = "*.csv";
    private string? _parameterFilter;
    private string? _selectedMachineId;

    public string ImportLog => _importLogBuilder.ToString();

    public ObservableCollection<string> MachineIds { get; set; } = new ObservableCollection<string>();

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

    public MachineDetailViewModel(
        ISearchService searchService,
        IImportService importService,
        IDefaultParametersViewModel defaultParametersViewModel,
        PathConfiguration pathConfiguration,
        IEventAggregator eventAggregator)
    {
        _searchService = searchService;
        _importService = importService;
        _defaultParametersViewModel = defaultParametersViewModel;
        _pathConfiguration = pathConfiguration;
        _eventAggregator = eventAggregator;
        Parameters = new CollectionViewSource { Source = _parameters };
        Results = new ObservableCollection<Parameter>();
        _eventAggregator.Subscribe<DefaultParameterSelectionChangedEvent>(_ => SearchResultsAsync());
    }

    public void LogException(string exceptionMessage)
    {
        _importLogBuilder.AppendLine(exceptionMessage);
        OnPropertyChanged(nameof(ImportLog));
    }

    public async Task InitializeAsync()
    {
        var allMachineIdsAsync = await _searchService.GetAllMachineIdsAsync().ConfigureAwait(true);
        allMachineIdsAsync.OrderBy(s => s).ToList().ForEach(m => MachineIds.Add(m));
        SelectedMachineId = MachineIds.FirstOrDefault();
        if (SelectedMachineId != null)
        {
            var allParametersAsync = _searchService.GetAllParametersByMachineIdAsync(SelectedMachineId);
            foreach (var p in allParametersAsync.ToList())
            {
                var parameterViewModel = new ParameterViewModel(p);
                parameterViewModel.OnIsSelectedChanged += (_, _) => Dispatcher.CurrentDispatcher.InvokeAsync(SearchResultsAsync);
                _parameters.Add(parameterViewModel);
            }
        }

        Parameters.Filter += FilterParameters;
        Parameters.View.Refresh();
        foreach (var file in Directory.GetFiles(_pathConfiguration.WatchPath, fileMask))
        {
            var stopwatch = Stopwatch.StartNew();
            _importLogBuilder.AppendLine($"Importing {file}.");
            OnPropertyChanged(nameof(ImportLog));
            var fileName = string.Empty;
            await Task.Run(() => { fileName = ImportFile(file); });
            _importLogBuilder.AppendLine($"{fileName} imported in {stopwatch.Elapsed.TotalSeconds:0.0} seconds.");
            OnPropertyChanged(nameof(ImportLog));
        }

        StartFileWatcher();
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

    private string ImportFile(string file)
    {
        var fileInfo = new FileInfo(file);
        _importService.ImportCsvData(fileInfo.FullName);
        File.Move(fileInfo.FullName, Path.Combine(_pathConfiguration.ArchivePath, fileInfo.Name), true);
        return fileInfo.Name;
    }

    private async Task SearchResultsAsync()
    {
        var parameters = _parameters.Where(p => p.IsSelected)
            .Select(p => p.Name)
            .Concat(_defaultParametersViewModel.GetSelectedDefaultParameters())
            .Distinct()
            .ToArray();

        var results = await _searchService.SearchEverywhereAsync(new[] { SelectedMachineId }, parameters).ConfigureAwait(true);

        Results.Clear();
        foreach (var searchResult in results)
        {
            foreach (var parameter in searchResult.Parameters)
            {
                Results.Add(parameter);
            }
        }
    }

    private void StartFileWatcher()
    {
        Task.Run(
            () =>
            {
                var watcher = new FileSystemWatcher(_pathConfiguration.WatchPath);
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = fileMask;

                // Add event handlers
                watcher.Created += OnCreated;

                // Begin watching
                watcher.EnableRaisingEvents = true;

                while (true)
                {
                }

                void OnCreated(object source, FileSystemEventArgs eventArgs)
                {
                    var maxRetries = 10;
                    var retryDelayMs = 1000; // 1 second delay between retries

                    var retryCount = 0;
                    var fileAccessible = false;
                    try
                    {
                        while (retryCount < maxRetries && !fileAccessible)
                        {
                            try
                            {
                                var stopwatch = Stopwatch.StartNew();
                                var destFileName = Path.Combine(_pathConfiguration.TempPath, eventArgs.Name);
                                File.Copy(eventArgs.FullPath, destFileName, true);
                                _importLogBuilder.AppendLine($"Importing {eventArgs.Name}.");
                                OnPropertyChanged(nameof(ImportLog));
                                var fileName = ImportFile(destFileName);
                                File.Delete(eventArgs.FullPath);
                                File.Delete(destFileName);
                                fileAccessible = true;
                                _importLogBuilder.AppendLine($"{fileName} imported in {stopwatch.Elapsed.TotalSeconds:0.0} seconds.");
                                OnPropertyChanged(nameof(ImportLog));
                            }
                            catch (IOException)
                            {
                                retryCount++;
                                Thread.Sleep(retryDelayMs);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            });
    }
}