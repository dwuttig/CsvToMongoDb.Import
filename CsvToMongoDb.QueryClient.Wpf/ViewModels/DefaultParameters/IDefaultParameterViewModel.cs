namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public interface IDefaultParameterViewModel
{
    string Name { get; set; }

    bool IsSelected { get; set; }

    string Key { get; }
}