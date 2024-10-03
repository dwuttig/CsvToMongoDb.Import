namespace CsvToMongoDb.QueryClient.Wpf.Infrastructure;

public class DefaultParameterSelectionChangedEvent : IEventArgs
{
    public static DefaultParameterSelectionChangedEvent Default = new DefaultParameterSelectionChangedEvent();
}