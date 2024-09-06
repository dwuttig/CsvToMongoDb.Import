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

    public IDocumentCollection GetOrCreateCollection(string collectionName)
    {
        if (_database.GetCollection<BsonDocument>(collectionName) is { } collection)
        {
            return new MongoDocumentCollection(collection);
        }

        _database.CreateCollection(collectionName);

        return new MongoDocumentCollection(_database.GetCollection<BsonDocument>(collectionName));
    }

    public void InsertDocument(string collectionName, BsonDocument document)
    {
        var collection = GetOrCreateCollection(collectionName);
        collection.InsertOne(document);
    }

    public ParameterResult SearchDocument(string field, string value, string collectionName)
    {
        var collection = GetOrCreateCollection(collectionName);
        var filter = Builders<BsonDocument>.Filter.Eq(field, value);
        var find = collection.Find(filter);
        var results = find.ToList();
        if (results.Count == 0)
        {
            return ParameterResult.Empty;
        }
        var valueResult = results.Select(d => d["Value"]).First().AsString;
        var unit = results.Select(d => d["Unit"]).First().AsString;
        var name = results.Select(d => d["Name"]).First().AsString;
        var qualifiedName = results.Select(d => d["Qualified Name"]).First().AsString;
        

        return new ParameterResult(name, qualifiedName, valueResult, unit);
    }

    public IList<string> GetAllFields(string collectionName)
    {
        var collection = GetOrCreateCollection(collectionName);
        var results = collection.Distinct("Name", new BsonDocument()).ToList();
        return results;
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