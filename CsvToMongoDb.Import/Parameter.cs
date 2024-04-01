namespace CsvToMongoDb.Import;

public record Parameter
{
    public string Name { get; }

    public string QualifiedName { get; }

    public string Unit { get; }

    public string Value { get; }

    public Parameter(string name, string qualifiedName, string value, string unit)
    {
        Name = name;
        Value = value;
        Unit = unit;
        QualifiedName = qualifiedName;
    }
}