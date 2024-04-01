namespace CsvToMongoDb.Import;

public interface ISearchService
{
    Task<IEnumerable<string>> GetAllMachineIdsAsync();

    Task<IEnumerable<string>> GetAllParametersByMachineIdAsync(string machineId);

    Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] returnFields);
}