namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public interface IDefaultParameterViewModelFactory
{
    IDefaultParameterViewModel Create(string key, string name);
}