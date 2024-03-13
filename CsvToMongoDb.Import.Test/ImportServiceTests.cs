using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Shouldly;

namespace CsvToMongoDb.Import.Test;

[TestFixture]
public class ImportServiceTests
{
    private readonly IImportService _importService = new ImportService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<ImportService>>());
    private readonly ISearchService _searchService = new SearchService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<SearchService>>());
    private readonly ICleanupService _cleanupService = new CleanupService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<ImportService>>());

    [TearDown]
    public void AfterEachTest()
    {
        _cleanupService.DeleteAllAsync();
    }

    [Test]
    public void ImportCsvData_PositiveCase()
    {
        // Arrange

        // Act
        _importService.ImportCsvData("Resources/positive_test_data.csv");

        // Assert
        // Add assertions to check if data was imported correctly
        _searchService.GetAll().Count.ShouldBe(5);
    }

    [Test]
    public void ImportCsvData_EmptyCsvFile()
    {
        // Arrange

        // Act
        _importService.ImportCsvData("Resources/empty_test_data.csv");

        // Assert
        // Add assertions to check if no data was imported
        _searchService.GetAll().Count.ShouldBe(0);
    }

    [Test]
    public void ImportCsvData_NegativeCase()
    {
        // Arrange
        
        // Act
        // Assert
        // Add assertions to check for appropriate exception handling
        // For example, check if an IOException was thrown
        Assert.Throws<IOException>(() => _importService.ImportCsvData("Resources/non_existent_file.csv"));
    }
}