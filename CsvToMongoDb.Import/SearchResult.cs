namespace CsvToMongoDb.Import;

public record SearchResult
{
    public SearchResult(string blockNr, IList<Parameter> parameters)
    {
        Name = blockNr;
        Parameters = parameters;
    }

    public string Name { get; }

    public IList<Parameter> Parameters { get; }
}