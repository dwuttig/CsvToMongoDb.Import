namespace CsvToMongoDb.Import;

public record Parameter
{
    public Parameter(string name, string qualifiedName, string value, string unit)
    {
        Name = name;
        Value = value;
        Unit = unit;
        QualifiedName = qualifiedName;
    }

    public string Name { get; }

    public string Value { get; }

    public string QualifiedName { get; }

    public string Unit { get; }
}