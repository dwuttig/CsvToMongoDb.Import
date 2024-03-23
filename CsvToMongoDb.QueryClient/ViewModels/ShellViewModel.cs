using Caliburn.Micro;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.ViewModel;

namespace CsvToMongoDb.QueryClient.ViewModels;

public class ShellViewModel : Screen, IShellViewModel
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