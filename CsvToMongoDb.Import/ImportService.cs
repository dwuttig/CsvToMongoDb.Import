using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import
{
    public class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        private IMongoDatabase _database;

        public ImportService(MongoClient mongoClient, string dataBaseName, ILogger<ImportService> logger)
        {
            _database = mongoClient.GetDatabase(dataBaseName);
            _logger = logger;
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
            _database.RenameCollection(collectionName, blockNr, new RenameCollectionOptions() { DropTarget = true });
            _logger.LogInformation($"CSV data imported for BlockNr {blockNr} into MongoDB successfully.");
        }

        private static string SearchBlockNr(string field, string value, IMongoCollection<BsonDocument> collection)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(field, value);
            var find = collection.Find(filter);
            var results = find.ToList();

            return results.Select(d => d["Value"]).First().AsString;
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
    }
}