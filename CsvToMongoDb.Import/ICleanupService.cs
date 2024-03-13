namespace CsvToMongoDb.Import;

public interface ICleanupService
{
    Task DeleteAllAsync();
}