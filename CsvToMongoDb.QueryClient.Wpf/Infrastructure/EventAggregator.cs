namespace CsvToMongoDb.QueryClient.Wpf.Infrastructure;

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<Func<object, Task>>> _subscribers = new Dictionary<Type, List<Func<object, Task>>>();

    public void Publish<T>(T eventArgs)
        where T : class, IEventArgs
    {
        PublishAsyncActions(eventArgs);
    }

    private async Task PublishAsyncActions(object eventArgs)
    {
        var eventType = eventArgs.GetType();
        if (_subscribers.TryGetValue(eventType, out var subscribers))
        {
            foreach (var subscriber in subscribers)
            {
                await subscriber(eventArgs).ConfigureAwait(false);
            }
        }
    }

    public void Subscribe<T>(Func<T, Task> asyncAction)
        where T : class, IEventArgs
    {
        var eventType = typeof(T);
        if (!_subscribers.TryGetValue(eventType, out var subscribers))
        {
            subscribers = new List<Func<object, Task>>();
            _subscribers.Add(eventType, subscribers);
        }

        subscribers.Add(async eventArgs => await asyncAction((T)eventArgs));
    }
}