using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class CleanupService : ICleanupService
{
    private readonly ILogger<ImportService> _logger;
    private IMongoDatabase _database;

    public CleanupService(MongoClient mongoClient, string dataBaseName, ILogger<ImportService> logger)
    {
        _database = mongoClient.GetDatabase(dataBaseName);
        _logger = logger;
    }

    public async Task DeleteAllAsync()
    {
        var collectionNames = await _database.ListCollectionNamesAsync().ConfigureAwait(false);
        foreach (var collectionName in collectionNames.ToList())
        {
            _database.DropCollection(collectionName);
            _logger.LogInformation($"{collectionName} deleted.");
        }
    }
}