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

    public IEnumerable<string> GetAllMachineIds()
    {
        return _database.ListCollectionNames().ToList();
    }
    
    public IEnumerable<string> GetAllParameters()
    {
        var collections = _database.ListCollectionNames().ToList();

        IList<string> parameters = new List<string>();
        foreach (var collectionName in collections)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            foreach (var parameter in collection.Distinct<string>("Name", new BsonDocument()).ToList())
            {
                parameters.Add(parameter);
            }
        }
        
        return parameters;
    }

    public List<SearchResult> SearchEverywhere(string[] blockNr, params string[] returnFields)
    {
        var collections = _database.ListCollectionNames().ToList().Where(c=>blockNr.Contains(c));
        List<SearchResult> result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            IList<Parameter> parameters = new List<Parameter>();
            foreach (var returnField in returnFields)
            {
                var paramResult = Search(returnField, collection);
                var value = paramResult.Select(d => d["Value"]).First().AsString;
                var qualifiedName = paramResult.Select(d => d["Qualified Name"]).First().AsString;
                var unit = paramResult.Select(d => d["Unit"]).First().AsString;
                parameters.Add(new Parameter(returnField, qualifiedName, value, unit));
            }

            result.Add(new SearchResult(collectionName, parameters));
        }
        
        return result;
    }

    private List<BsonDocument> Search(string value, IMongoCollection<BsonDocument> collection)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("Name", value);
        var find = collection.Find(filter);
        var results = find.ToList();

        _logger.LogInformation($"Search results for parameter = {value}:");
        return results;
    }
}