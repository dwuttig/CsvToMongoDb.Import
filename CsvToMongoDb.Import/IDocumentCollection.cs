using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public interface IDocumentCollection
{
    // Add other methods as needed...
    List<string> Distinct(string field, BsonDocument filter);

    List<BsonDocument> Find(FilterDefinition<BsonDocument> filter);

    void InsertOne(BsonDocument document);
}