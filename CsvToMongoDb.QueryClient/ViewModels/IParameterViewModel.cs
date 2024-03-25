namespace CsvToMongoDb.QueryClient.ViewModels;

public interface IParameterViewModel
{
    public bool IsSelected { get; set; }

    public string Name { get; set; }
}