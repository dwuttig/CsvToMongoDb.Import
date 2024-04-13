using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class Repository : IRepository
{
    private readonly IMongoDatabase _database;

    public Repository(IMongoDatabase mongoDatabase)
    {
        _database = mongoDatabase;
    }

    public IMongoCollection<BsonDocument> GetOrCreateCollection(string collectionName)
    {
        if (_database.GetCollection<BsonDocument>(collectionName) is { } collection)
        {
            return collection;
        }

        _database.CreateCollection(collectionName);

        return _database.GetCollection<BsonDocument>(collectionName);
    }

    public void InsertDocument(string collectionName, BsonDocument document)
    {
        var collection = GetOrCreateCollection(collectionName);
        collection.InsertOne(document);
    }

    public string SearchDocument(string field, string value, string collectionName)
    {
        var collection = GetOrCreateCollection(collectionName);
        var filter = Builders<BsonDocument>.Filter.Eq(field, value);
        var find = collection.Find(filter);
        var results = find.ToList();

        return results.Select(d => d["Value"]).First().AsString;
    }

    public async Task<IEnumerable<string>> GetAllCollectionNamesAsync()
    {
        return await _database.ListCollectionNames().ToListAsync();
    }

    public void RenameCollection(string oldName, string newName)
    {
        _database.RenameCollection(oldName, newName, new RenameCollectionOptions { DropTarget = true });
    }
}