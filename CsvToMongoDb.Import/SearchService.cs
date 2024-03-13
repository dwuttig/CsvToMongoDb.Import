using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class SearchService : ISearchService
{
    private readonly ILogger<SearchService> _logger;
    private IMongoDatabase _database;

    public SearchService(MongoClient mongoClient, string dataBaseName, ILogger<SearchService> logger)
    {
        _database = mongoClient.GetDatabase(dataBaseName);
        _logger = logger;
    }

    private List<BsonDocument> Search(string field, string value, IMongoCollection<BsonDocument> collection)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(field, value);
        var find = collection.Find(filter);
        var results = find.ToList();

        _logger.LogInformation($"Search results for {field} = {value}:");
        return results;
    }

    public List<SearchResult> SearchEverywhere(string searchField, string searchValue)
    {
        var collections = _database.ListCollectionNames().ToList();
        List<SearchResult> result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            result.AddRange(Search(searchField, searchValue, collection).Select(r => new SearchResult(collectionName)));
        }

        _logger.LogInformation($"Search results for {searchField} = {searchValue}: {result.Count}");
        return result;
    }

    public List<SearchResult> GetAll()
    {
        var collections = _database.ListCollectionNames().ToList();
        List<SearchResult> result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            result.AddRange(collection.Find(FilterDefinition<BsonDocument>.Empty).ToList().Select(r => new SearchResult(collectionName)));
        }

        return result;
    }
}

public record SearchResult
{
    public SearchResult(string name)
    {
        Name = name;
    }

    public string Name { get; }
}