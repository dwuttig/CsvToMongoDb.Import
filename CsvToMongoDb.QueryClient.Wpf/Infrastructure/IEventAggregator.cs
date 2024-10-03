namespace CsvToMongoDb.QueryClient.Wpf.Infrastructure;

public interface IEventAggregator
{
    void Publish<T>(T eventArgs) where T : class, IEventArgs;

    void Subscribe<T>(Func<T, Task> asyncAction) where T : class, IEventArgs;
}