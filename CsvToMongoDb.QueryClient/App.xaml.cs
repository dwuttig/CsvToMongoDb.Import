﻿using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CsvToMongoDb.Import;
using CsvToMongoDb.QueryClient.ViewModels;
using CsvToMongoDb.QueryClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace CsvToMongoDb.QueryClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
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
                    .AddSingleton(typeof(IMongoDatabase), new MongoClient(configuration["MongoDbConnectionString"]).GetDatabase(configuration["MongoDbDatabase"]))
                    .AddSingleton<IShellViewModel,ShellViewModel>()
                    .BuildServiceProvider());

            MainWindow.DataContext = Ioc.Default.GetService<IShellViewModel>();
            
            MainWindow.Show();
        }
    }
}