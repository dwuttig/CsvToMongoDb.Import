namespace CsvToMongoDb.Import;

public class ImportServiceFactory : IImportServiceFactory
{
    private readonly IRepository _repository;

    public ImportServiceFactory(IRepository repository)
    {
        _repository = repository;
    }

    public ImportService Create()
    {
        return new ImportService(_repository);
    }
}