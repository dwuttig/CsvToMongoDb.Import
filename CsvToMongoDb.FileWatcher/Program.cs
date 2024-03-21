// See https://aka.ms/new-console-template for more information

using CsvToMongoDb.Import;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

internal class Program
{
    private static ImportService _importService;
    private static IConfigurationRoot _configuration;

    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        _configuration = builder.Build();
        var watchPath = _configuration["WatchPath"];

        _importService = new ImportService(new MongoClient(_configuration["mongoDbClient"]), _configuration["mongoDbName"]);

        var watcher = new FileSystemWatcher(watchPath);
        watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        // Only watch text files.
        watcher.Filter = "*.*";

        // Add event handlers
        watcher.Created += OnCreated;

        // Begin watching
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Press 'q' to quit the sample.");
        while (Console.Read() != 'q')
        {
        }

        static void OnCreated(object source, FileSystemEventArgs e)
        {
            try
            {
                _importService.ImportCsvData(e.FullPath);
                File.Move(e.FullPath, Path.Combine(_configuration["ArchivePath"], e.Name), true);
                Console.WriteLine($"{e.Name} imported.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}