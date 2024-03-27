// See https://aka.ms/new-console-template for more information

using CsvToMongoDb.Import;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CsvToMongoDb.FileWatcher;

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

        MongoClient mongoClient = new MongoClient(_configuration["mongoDbClient"]);
        string dataBaseName = _configuration["mongoDbName"];
        _importService = new ImportService(mongoClient.GetDatabase(dataBaseName));

        foreach (var file in Directory.GetFiles(_configuration["WatchPath"], "*.csv"))
        {
            ImportFile(file);
        }

        var watcher = new FileSystemWatcher(watchPath);
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;

        // Only watch text files.
        watcher.Filter = "*.csv";

        // Add event handlers
        watcher.Created += OnCreated;

        // Begin watching
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Press 'q' to quit the sample.");
        while (Console.Read() != 'q')
        {
        }

        static void OnCreated(object source, FileSystemEventArgs eventArgs)
        {
            var maxRetries = 10;
            var retryDelayMs = 1000; // 1 second delay between retries

            var retryCount = 0;
            var fileAccessible = false;
            try
            {
                while (retryCount < maxRetries && !fileAccessible)
                {
                    try
                    {
                        var destFileName = Path.Combine(_configuration["TempPath"], eventArgs.Name);
                        File.Copy(eventArgs.FullPath, destFileName, true);

                        ImportFile(destFileName);
                        File.Delete(eventArgs.FullPath);
                        File.Delete(destFileName);
                        fileAccessible = true;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Attempt {retryCount + 1}: File is not accessible - {ex.Message}");
                        retryCount++;
                        Thread.Sleep(retryDelayMs); // Wait before retrying
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }

    private static void ImportFile(string file)
    {
        var fileInfo = new FileInfo(file);
        _importService.ImportCsvData(fileInfo.FullName);
        File.Move(fileInfo.FullName, Path.Combine(_configuration["ArchivePath"], fileInfo.Name), true);
        Console.WriteLine($"{fileInfo.Name} imported.");
    }
}