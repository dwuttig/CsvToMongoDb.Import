namespace CsvToMongoDb.Import;

public interface IImportService
{
    void ImportCsvData(string csvFilePath);

    void ImportCsvDataInNewCollection(string csvFilePath);
}