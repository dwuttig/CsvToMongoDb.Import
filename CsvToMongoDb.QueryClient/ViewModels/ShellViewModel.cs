using CommunityToolkit.Mvvm.ComponentModel;
using CsvToMongoDb.Import;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ShellViewModel : ObservableObject, IShellViewModel
{
    private readonly ISearchService _searchService;

    public ShellViewModel(ISearchService searchService)
    {
        _searchService = searchService;
        MachineIds = _searchService.GetAllMachineIds().OrderBy(s => s).ToList();
    }

    public IList<ParameterViewModel> Parameters { get; set; } = new List<ParameterViewModel>();

    public IList<string> MachineIds { get; set; }

    public string SelectedMachineId { get; set; }
}