namespace CsvToMongoDb.Import;

public record Parameter
{
    public string MachineId { get; }

    public string Name { get; }

    public string QualifiedName { get; }

    public string Unit { get; }

    public string Value { get; }

    public Parameter(string machineId, string name, string qualifiedName, string value, string unit)
    {
        MachineId = machineId;
        Name = name;
        Value = value;
        Unit = unit;
        QualifiedName = qualifiedName;
    }
}