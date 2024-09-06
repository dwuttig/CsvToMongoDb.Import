namespace CsvToMongoDb.Import;

public interface IImportServiceFactory
{
    ImportService Create();
}