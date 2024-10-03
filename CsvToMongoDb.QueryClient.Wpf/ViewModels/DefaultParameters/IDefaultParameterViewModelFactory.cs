namespace CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;

public interface IDefaultParameterViewModelFactory
{
    DefaultParameterViewModel Create(string key, string name);
}