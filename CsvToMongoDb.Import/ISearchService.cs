namespace CsvToMongoDb.Import;

public interface ISearchService
{
    List<SearchResult> SearchEverywhere(string searchField, string searchValue);

    List<SearchResult> GetAll();
}