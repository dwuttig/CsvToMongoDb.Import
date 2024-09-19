using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.Wpf.Configuration;
using CsvToMongoDb.QueryClient.Wpf.ViewModels;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.DefaultParameters;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.MachineDetail;
using CsvToMongoDb.QueryClient.Wpf.ViewModels.ParameterSearch;
using CsvToMongoDb.QueryClient.Wpf.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace CsvToMongoDb.QueryClient.Wpf;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string AppSettingsFile = "Configuration/appsettings.json";
    private const string Log4NetConfigFile = "Configuration/log4net.config";

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            if (MainWindow is null)
            {
                if (!File.Exists(AppSettingsFile))
                {
                    throw new FileNotFoundException($"The file '{AppSettingsFile}' was not found.");
                }

                IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                configurationBuilder.AddJsonFile(AppSettingsFile, false, true);
                var configuration = configurationBuilder.Build();

                // Register services
                var connectionString = configuration["MongoDbConnectionString"] ?? throw new InvalidOperationException("MongoDbConnectionString is missing in the configuration.");
                var databaseName = configuration["MongoDbDatabase"] ?? throw new InvalidOperationException("MongoDbDatabase is missing in the configuration.");
                var watchPath = configuration["WatchPath"] ?? throw new InvalidOperationException("WatchPath is missing in the configuration.");
                var tempPath = configuration["TempPath"] ?? throw new InvalidOperationException("TempPath is missing in the configuration.");
                var archivePath = configuration["ArchivePath"] ?? throw new InvalidOperationException("ArchivePath is missing in the configuration.");

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                        .AddLogging(builder => builder.AddLog4Net(Log4NetConfigFile))
                        .AddSingleton<IRepository, Repository>()
                        .AddSingleton(typeof(IDefaultParameterReader), new DefaultParameterReader(configuration))
                        .AddSingleton<ISearchService, SearchService>()
                        .AddSingleton<IImportService, ImportService>()
                        .AddSingleton<IUserSettingsService, UserSettingsService>()
                        .AddSingleton<IThemeService, ThemeService>()
                        .AddSingleton(typeof(PathConfiguration), new PathConfiguration(watchPath, tempPath, archivePath))
                        .AddSingleton(typeof(IMongoDatabase), new MongoClient(connectionString).GetDatabase(databaseName))
                        .AddSingleton<IMachineDetailViewModel, MachineDetailViewModel>()
                        .AddSingleton<IParameterSearchViewModel, ParameterSearchViewModel>()
                        .AddSingleton<IDefaultParametersViewModel, DefaultParametersViewModel>()
                        .AddSingleton<IShellViewModel, ShellViewModel>()
                        .BuildServiceProvider());

                SetTheme();
                var shellViewModel = Ioc.Default.GetService<IShellViewModel>() ?? throw new InvalidOperationException("IShellViewModel service not found.");
                MainWindow = new ShellView();
                MainWindow.DataContext = shellViewModel;
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => shellViewModel.MachineDetailViewModel.InitializeAsync().ConfigureAwait(true)));
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => shellViewModel.ParameterSearchViewModel.InitializeAsync().ConfigureAwait(true)));
                MainWindow.Show();
            }
        }
        catch (Exception ex)
        {
            var shellViewModel = Ioc.Default.GetService<IShellViewModel>() ?? throw new InvalidOperationException("IShellViewModel service not found.");
            shellViewModel.MachineDetailViewModel.LogException($"Error during startup: {ex.Message}");
        }
    }

    private static void SetTheme()
    {
        var userSettingsService = Ioc.Default.GetRequiredService<IUserSettingsService>() ?? throw new InvalidOperationException("IUserSettingsService service not found.");
        var themeService = Ioc.Default.GetRequiredService<IThemeService>() ?? throw new InvalidOperationException("IThemeService service not found.");

        var userSettings = userSettingsService.LoadUserSettings();
        themeService.ChangeTheme(userSettings.SelectedTheme);
    }
}