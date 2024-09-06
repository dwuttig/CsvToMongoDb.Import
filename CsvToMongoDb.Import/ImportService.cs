using MongoDB.Bson;

namespace CsvToMongoDb.Import;

public class ImportService : IImportService
{
    private readonly IRepository _repository;

    public ImportService(IRepository repository)
    {
        _repository = repository;
    }

    public void ImportCsvData(string csvFilePath)
    {
        if (!File.Exists(csvFilePath))
        {
            throw new IOException("File not found");
        }

        var collectionName = new FileInfo(csvFilePath).Name;

        var csvLines = File.ReadAllLines(csvFilePath);
        var header = csvLines.FirstOrDefault();
        if (header == null)
        {
            return;
        }

        var records = csvLines.Skip(1);

        foreach (var record in records)
        {
            var document = new BsonDocument();
            var values = record.Split(';');

            for (var i = 0; i < values.Length; i++)
            {
                document.Add(header.Split(';')[i].Trim(), values[i].Trim());
            }

            _repository.InsertDocument(collectionName, document);
        }

        var blockNr = _repository.SearchDocument("Name", "BlockNr", collectionName);
        _repository.RenameCollection(collectionName, blockNr.Name);
    }
}