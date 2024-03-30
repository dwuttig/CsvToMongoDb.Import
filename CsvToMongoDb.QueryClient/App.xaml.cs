using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.ViewModels;
using CsvToMongoDb.QueryClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace CsvToMongoDb.QueryClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            // Ensure the UI is initialized
            if (MainWindow is null)
            {
                MainWindow = new ShellView();
                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = configurationBuilder.Build();

                // Register services
                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                        .AddLogging(builder => builder.AddLog4Net("Configuration/log4net.config"))
                        .AddSingleton<ISearchService, SearchService>()
                        .AddSingleton<IImportService, ImportService>()
                        .AddSingleton(typeof(IMongoDatabase), new MongoClient(configuration["MongoDbConnectionString"]).GetDatabase(configuration["MongoDbDatabase"]))
                        .AddSingleton<IShellViewModel, ShellViewModel>()
                        .BuildServiceProvider());

                var shellViewModel = Ioc.Default.GetService<IShellViewModel>();
                MainWindow.DataContext = shellViewModel;
                Dispatcher.BeginInvoke(DispatcherPriority.Background , new Action(() => shellViewModel.InitializeAsync().ConfigureAwait(true)));
                MainWindow.Show();
            }
        }
        catch (Exception ex)
        {
            var shellViewModel = Ioc.Default.GetService<IShellViewModel>();
            shellViewModel?.LogException($"Error during startup: {ex.Message}");
        }
    }
}