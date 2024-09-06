namespace CsvToMongoDb.Import;

public interface ISearchService
{
    Task<IEnumerable<string>> GetAllMachineIdsAsync();

    IEnumerable<string> GetAllParametersByMachineIdAsync(string machineId);

    Task<List<SearchResult>> SearchEverywhereAsync(string?[] blockNr, params string[] parameterNames);

    Task<IEnumerable<string>> GetAllParameters();

    Task<List<SearchResult>> SearchByTypeAsync(IList<MachineType> machineTypes, params string[] parameterNames);
}