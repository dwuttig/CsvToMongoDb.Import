namespace CsvToMongoDb.Import;

public interface ISearchService
{
    Task<IEnumerable<string>> GetAllMachineIdsAsync();

    IEnumerable<string> GetAllParametersByMachineIdAsync(string machineId);

    Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] returnFields);
}