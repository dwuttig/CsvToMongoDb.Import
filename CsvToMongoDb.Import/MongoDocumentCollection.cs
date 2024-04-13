using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public class MongoDocumentCollection : IDocumentCollection
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public MongoDocumentCollection(IMongoCollection<BsonDocument> collection)
    {
        _collection = collection;
    }

    public void InsertOne(BsonDocument document)
    {
        _collection.InsertOne(document);
    }

    public List<string> Distinct(string field, BsonDocument filter)
    {
        return _collection.Distinct<string>(field, filter).ToList();
    }

    public List<BsonDocument> Find(FilterDefinition<BsonDocument> filter)
    {
        return _collection.Find(filter).ToList();
    }
}