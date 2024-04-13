using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace CsvToMongoDb.Import;

public class SearchService : ISearchService
{
    private readonly IRepository _repository;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IRepository repository, ILogger<SearchService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetAllMachineIdsAsync()
    {
        return await _repository.GetAllCollectionNamesAsync();
    }

    public IEnumerable<string> GetAllParametersByMachineIdAsync(string machineId)
    {
        var collection = _repository.GetOrCreateCollection(machineId);
        return collection.Distinct("Name", new BsonDocument());
    }

    public async Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] returnFields)
    {
        var collections = (await _repository.GetAllCollectionNamesAsync()).Where(c => blockNr.Contains(c));
        var result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var parameters = new List<Parameter>();
            foreach (var returnField in returnFields)
            {
                var value = _repository.SearchDocument("Name", returnField, collectionName);
                var qualifiedName = _repository.SearchDocument("Name", returnField, collectionName);
                var unit = _repository.SearchDocument("Name", returnField, collectionName);
                parameters.Add(new Parameter(returnField, qualifiedName, value, unit));
            }

            result.Add(new SearchResult(collectionName, parameters));
        }

        return result;
    }
}