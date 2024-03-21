namespace CsvToMongoDb.Import;

public interface ISearchService
{
    List<SearchResult> SearchEverywhere(string[] blockNr, params string[] returnFields);
}