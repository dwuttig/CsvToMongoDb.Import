using Caliburn.Micro;

namespace CsvToMongoDb.QueryClient.ViewModel;

public class ParameterViewModel : Screen, IParameterViewModel
{
    public bool IsSelected { get; set; }

    public string Name { get; set; }
}