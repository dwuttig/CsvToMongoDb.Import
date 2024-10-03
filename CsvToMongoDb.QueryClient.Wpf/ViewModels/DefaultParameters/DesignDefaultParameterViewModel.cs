namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public class DesignDefaultParameterViewModel : IDefaultParameterViewModel
{
    public string Name { get; set; }

    public bool IsSelected { get; set; }

    public DesignDefaultParameterViewModel(string name)
    {
        Name = name;
    }
}