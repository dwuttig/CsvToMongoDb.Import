namespace CsvToMongoDb.Import;

public record ParameterResult
{
    public string Name { get; init; }
    
    public string QualifiedName { get; init; }

    public string Value { get; init; }

    public string Unit { get; init; }

    public static ParameterResult Empty => new ParameterResult(string.Empty, string.Empty, string.Empty, string.Empty);

    public ParameterResult(string name, string qualifiedName, string value, string unit)
    {
        Name = name;
        QualifiedName = qualifiedName;
        Value = value;
        Unit = unit;
    }
}