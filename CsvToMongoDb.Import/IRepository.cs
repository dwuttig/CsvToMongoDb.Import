using MongoDB.Bson;

namespace CsvToMongoDb.Import;

public interface IRepository
{
    Task<IEnumerable<string>> GetAllCollectionNamesAsync();

    IDocumentCollection GetOrCreateCollection(string collectionName);

    void InsertDocument(string collectionName, BsonDocument document);

    void RenameCollection(string oldName, string newName);

    ParameterResult SearchDocument(string field, string value, string collectionName);

    IList<string> GetAllFields(string collectionName);
}