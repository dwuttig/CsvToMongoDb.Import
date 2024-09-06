using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

public class DesignParameterSearchViewModel : IParameterSearchViewModel
{
    public bool IsSoftStarter { get; set; }

    public bool IsDrive { get; set; }

    public bool IsGtStarter { get; set; }

    public CollectionViewSource Parameters { get; init; } = new CollectionViewSource();

    public string? ParameterFilter { get; set; }

    public AsyncRelayCommand SearchCommand { get; }

    public DesignParameterSearchViewModel()
    {
        SearchCommand = new AsyncRelayCommand(Search);
    }

    private static Task Search()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}