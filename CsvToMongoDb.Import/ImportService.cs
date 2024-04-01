using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class ImportService : IImportService
{
    private readonly IMongoDatabase _database;

    public ImportService(IMongoDatabase mongoDatabase)
    {
        _database = mongoDatabase;
    }

    public void ImportCsvData(string csvFilePath)
    {
        if (!File.Exists(csvFilePath))
        {
            throw new IOException("File not found");
        }

        var collectionName = new FileInfo(csvFilePath).Name;
        var collection = GetOrCreateCollection(collectionName);

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

            collection.InsertOne(document);
        }

        var blockNr = SearchBlockNr("Name", "BlockNr", collection);
        _database.RenameCollection(collectionName, blockNr, new RenameCollectionOptions { DropTarget = true });
    }

    private IMongoCollection<BsonDocument> GetOrCreateCollection(string collectionName)
    {
        if (_database.GetCollection<BsonDocument>(collectionName) is { } collection)
        {
            return collection;
        }

        _database.CreateCollection(collectionName);

        return _database.GetCollection<BsonDocument>(collectionName);
    }

    private static string SearchBlockNr(string field, string value, IMongoCollection<BsonDocument> collection)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(field, value);
        var find = collection.Find(filter);
        var results = find.ToList();

        return results.Select(d => d["Value"]).First().AsString;
    }
}