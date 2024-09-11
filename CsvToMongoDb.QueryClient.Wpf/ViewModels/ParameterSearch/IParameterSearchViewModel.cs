using System.Collections.ObjectModel;
using System.Windows.Data;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;

public interface IParameterSearchViewModel
{
    bool IsSoftStarter { get; set; }

    bool IsDrive { get; set; }

    bool IsGtStarter { get; set; }

    CollectionViewSource Parameters { get; init; }

    string? ParameterFilter { get; set; }

    ObservableCollection<Parameter> Results { get; init; }

    Task InitializeAsync();
}