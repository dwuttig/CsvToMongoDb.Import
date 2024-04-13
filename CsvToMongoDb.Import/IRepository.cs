using MongoDB.Bson;
using MongoDB.Driver;

namespace CsvToMongoDb.Import;

public interface IRepository
{
    Task<IEnumerable<string>> GetAllCollectionNamesAsync();

    IMongoCollection<BsonDocument> GetOrCreateCollection(string collectionName);

    void InsertDocument(string collectionName, BsonDocument document);

    void RenameCollection(string oldName, string newName);

    string SearchDocument(string field, string value, string collectionName);
}