using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class SearchService(IMongoDatabase mongoDatabase, ILogger<SearchService> logger) : ISearchService
{
    public async Task<IEnumerable<string>> GetAllMachineIdsAsync()
    {
        return await (await mongoDatabase.ListCollectionNamesAsync().ConfigureAwait(false)).ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAllParametersByMachineIdAsync(string machineId)
    {
        var collections = await (await mongoDatabase.ListCollectionNamesAsync()).ToListAsync().ConfigureAwait(false);

        var parameters = new List<string>();
        foreach (var collectionName in collections.Where(c => c == machineId))
        {
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
            foreach (var parameter in collection.Distinct<string>("Name", new BsonDocument()).ToList())
            {
                parameters.Add(parameter);
            }
        }
        return parameters;
    }

    public async Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] returnFields)
    {
        var collectionNamesAsync = await mongoDatabase.ListCollectionNamesAsync();
        var collections = (await collectionNamesAsync.ToListAsync()).Where(c => blockNr.Contains(c));
        var result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
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

        logger.LogInformation($"Search results for parameter = {value}:");
        return results;
    }
}