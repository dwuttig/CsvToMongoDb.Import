using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Shouldly;

namespace CsvToMongoDb.Import.Test;

[TestFixture]
public class SearchServiceTests
{
    private readonly IImportService _importService = new ImportService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<ImportService>>());
    private readonly ICleanupService _cleanupService = new CleanupService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<ImportService>>());
    private ISearchService _searchService;

    [OneTimeSetUp]
    public void Setup()
    {
        _searchService = new SearchService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<SearchService>>());
        _cleanupService.DeleteAllAsync();
    }

    [TearDown]
    public void AfterTestFixture()
    {
        _cleanupService.DeleteAllAsync();
    }

    [Test]
    public void Search_ValidInput_ReturnsResults()
    {
        // Arrange
        _importService.ImportCsvData("Resources/positive_test_data.csv");
        _importService.ImportCsvData("Resources/positive_test_data2.csv");
        var field = "P1";
        var value = "5";

        // Act
        var result = _searchService.SearchEverywhere(field, value);

        // Assert
        result.Count.ShouldBe(2);
    }

    [Test]
    public void Search_ValidInput_ReturnsMultipleResults()
    {
        // Arrange
        _importService.ImportCsvData("Resources/positive_test_data.csv");
        _importService.ImportCsvData("Resources/positive_test_data2.csv");
        var field = "P3";
        var value = "4";

        // Act
        var result = _searchService.SearchEverywhere(field, value);

        // Assert
        result.Count.ShouldBe(5);
    }

    [Test]
    public void Search_ValidInput_ReturnsMultipleCollectionResults()
    {
        // Arrange
        _importService.ImportCsvDataInNewCollection("Resources/positive_test_data.csv");
        _importService.ImportCsvDataInNewCollection("Resources/positive_test_data2.csv");
        var field = "P1";
        var value = "3";

        // Act
        var result = _searchService.SearchEverywhere(field, value);

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(r => r.Name.Equals("positive_test_data.csv"));
        result.ShouldContain(r => r.Name.Equals("positive_test_data2.csv"));
    }

    [Test]
    public void Search_ValidInput_ReturnsNoResults()
    {
        // Arrange
        _importService.ImportCsvData("Resources/positive_test_data.csv");
        _importService.ImportCsvData("Resources/positive_test_data2.csv");
        var field = "P1";
        var value = "11";

        // Act
        var result = _searchService.SearchEverywhere(field, value);

        // Assert
        result.Count.ShouldBe(0);
    }
}