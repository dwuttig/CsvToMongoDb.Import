namespace CsvToMongoDb.Import;

public record SearchResult
{
    public string Name { get; }

    public IList<Parameter> Parameters { get; }

    public SearchResult(string blockNr, IList<Parameter> parameters)
    {
        Name = blockNr;
        Parameters = parameters;
    }
}