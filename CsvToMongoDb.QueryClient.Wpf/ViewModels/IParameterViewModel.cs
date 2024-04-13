namespace CsvToMongoDb.QueryClient.Wpf.ViewModels;

public interface IParameterViewModel
{
    public bool IsSelected { get; set; }

    public string Name { get; set; }
}