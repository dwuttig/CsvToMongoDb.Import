namespace CsvToMongoDb.QueryClient.ViewModel;

public interface IParameterViewModel
{
    public bool IsSelected { get; set; }

    public string Name { get; set; }
}