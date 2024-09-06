using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

public interface IParameterSearchViewModel
{
    bool IsSoftStarter { get; set; }

    bool IsDrive { get; set; }

    bool IsGtStarter { get; set; }

    CollectionViewSource Parameters { get; init; }

    string? ParameterFilter { get; set; }

    AsyncRelayCommand SearchCommand { get; }

    Task InitializeAsync();
}