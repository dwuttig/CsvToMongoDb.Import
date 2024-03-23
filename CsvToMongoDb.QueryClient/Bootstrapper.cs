using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.ViewModel;
using CsvToMongoDb.QueryClient.ViewModels;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ILoggerFactory = DnsClient.Internal.ILoggerFactory;

namespace CsvToMongoDb.QueryClient;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container;

    public Bootstrapper()
    {
        Initialize();
    }

    protected override void Configure()
    {
        _container = new SimpleContainer();
        ^_container.RegisterInstance(typeof(ILoggerFactory), null, new LoggerFactory());
        _container.RegisterSingleton(typeof(ILogger<>), null, typeof(Logger<>));
        _container.RegisterInstance(typeof(MongoClient), null, new MongoClient("mongodb://localhost:27017"));
        _container.RegisterInstance(typeof(SimpleContainer), null, _container);
        _container.RegisterPerRequest(typeof(ISearchService), null, typeof(SearchService));
        _container.RegisterPerRequest(typeof(IParameterViewModel), null, typeof(ParameterViewModel));
        _container.RegisterSingleton(typeof(ShellViewModel), null, typeof(ShellViewModel));
    }
    
    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }


    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        
        DisplayRootViewForAsync<ShellViewModel>();
    }

    protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        base.OnUnhandledException(sender, e);
        MessageBox.Show(e.Exception.Message, "Unhandled Exception");
    }
}