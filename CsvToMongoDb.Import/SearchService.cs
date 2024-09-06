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

    public async Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] parameterNames)
    {
        var collections = (await _repository.GetAllCollectionNamesAsync()).Where(c => blockNr.Contains(c));
        var result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var parameters = new List<Parameter>();
            foreach (var parameterName in parameterNames)
            {
                var parameterResult = _repository.SearchDocument("Name", parameterName, collectionName);
                if (parameterResult.Equals(ParameterResult.Empty))
                {
                    continue;
                }
                parameters.Add(new Parameter(collectionName, parameterName, parameterResult.QualifiedName, parameterResult.Value, parameterResult.Unit));
            }

            result.Add(new SearchResult(collectionName, parameters));
        }

        return result;
    }

    public async Task<IEnumerable<string>> GetAllParameters()
    {
        var collections = await _repository.GetAllCollectionNamesAsync();

        var parameters = new List<string>();
        foreach (var collectionName in collections)
        {
            parameters.AddRange(_repository.GetAllFields(collectionName));
        }
        
        return parameters.Distinct();
    }

    public async Task<List<SearchResult>> SearchByTypeAsync(IList<MachineType> machineTypes, params string[] parameterNames)
    {
        var collections = await _repository.GetAllCollectionNamesAsync();
        var result = new List<SearchResult>();

        foreach (var collectionName in collections)
        {
            var parameters = new List<Parameter>();

            var machineType = _repository.SearchDocument("Name", "TypeCnfg", collectionName);
            if (!machineTypes.Contains((MachineType)int.Parse(machineType.Value)))
            {
                continue;
            }
            foreach (var parameterName in parameterNames)
            {
                var parameterResult = _repository.SearchDocument("Name", parameterName, collectionName);
                if (parameterResult.Equals(ParameterResult.Empty))
                {
                    continue;
                }
                parameters.Add(new Parameter(collectionName, parameterName, parameterResult.QualifiedName, parameterResult.Value, parameterResult.Unit));
            }

            result.Add(new SearchResult(collectionName, parameters));
        }

        return result;
    }
}