namespace CsvToMongoDb.Import;

public interface ISearchService
{
    Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] returnFields);

    Task<IEnumerable<string>> GetAllMachineIdsAsync();

    Task<IEnumerable<string>> GetAllParametersByMachineIdAsync(string machineId);
}