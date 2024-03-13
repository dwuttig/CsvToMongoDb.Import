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

            var collection = GetOrCreateCollection(new FileInfo(csvFilePath).Name);

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
                var values = record.Split(',');

                for (var i = 0; i < values.Length; i++)
                {
                    document.Add(header.Split(',')[i].Trim(), values[i].Trim());
                }

                collection.InsertOne(document);
            }

            _logger.LogInformation("CSV data imported into MongoDB successfully.");
        }

        private IMongoCollection<BsonDocument> GetOrCreateCollection(string collectioName)
        {
            if (_database.GetCollection<BsonDocument>(collectioName) is { } collection)
            {
                return collection;
            }
            _database.CreateCollection(collectioName);

            return _database.GetCollection<BsonDocument>(collectioName);
        }

        public void ImportCsvDataInNewCollection(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                throw new IOException("File not found");
            }

            var collectionName = new FileInfo(csvFilePath).Name;
            _database.CreateCollection(collectionName);
            var collection = _database.GetCollection<BsonDocument>(collectionName);
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
                var values = record.Split(',');

                for (var i = 0; i < values.Length; i++)
                {
                    document.Add(header.Split(',')[i].Trim(), values[i].Trim());
                }

                collection.InsertOne(document);
            }

            _logger.LogInformation("CSV data imported into MongoDB successfully.");
        }
    }
}