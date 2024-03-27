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
    private readonly string? _tempPath;

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
        _tempPath = configuration["TempPath"];
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

        StartFileWatcher();
    }

    private void StartFileWatcher()
    {
        Task.Run(
            () =>
            {
                var watcher = new FileSystemWatcher(_watchPath);
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.csv";

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
                                var destFileName = Path.Combine(_tempPath, eventArgs.Name);
                                File.Copy(eventArgs.FullPath, destFileName, true);
                                _importLogBuilder.AppendLine($"Importing {eventArgs.Name}.");
                                OnPropertyChanged(nameof(ImportLog));
                                var fileName = ImportFile(destFileName);
                                File.Delete(eventArgs.FullPath);
                                File.Delete(destFileName);
                                fileAccessible = true;
                                _importLogBuilder.AppendLine($"{fileName} imported.");
                                OnPropertyChanged(nameof(ImportLog));
                            }
                            catch (IOException ex)
                            {
                                retryCount++;
                                Thread.Sleep(retryDelayMs); // Wait before retrying
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