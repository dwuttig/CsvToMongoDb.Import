using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using Shouldly;

namespace CsvToMongoDb.Import.Test;

[TestFixture]
[NonParallelizable]
public class ImportServiceTests
{
    
    [TearDown]
    public void AfterEachTest()
    {
        _cleanupService.DeleteAllAsync();
    }

    [Test]
    public async Task GetMachineIds()
    {
        // Arrange
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (08_06_2019).csv");

        // Act
        var results = await _searchService.GetAllMachineIdsAsync().ConfigureAwait(false);

        // Assert
        results.Count().ShouldBe(2);
    }

    [Test]
    public void ImportCsvData_RealDataCase()
    {
        // Arrange

        // Act
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");

        // Assert
        // Add assertions to check if data was imported correctly
    }

    [Test]
    public async Task ImportMultipleFiles()
    {
        // Arrange

        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (08_06_2019).csv");

        //Act
        var results = await _searchService.SearchEverywhereAsync(new[] { "1081" }, "FirmwareVersionLIN7_000").ConfigureAwait(false);

        // Assert
        results.Count.ShouldBe(1);
    }

    private readonly IImportService _importService = new ImportService(new MongoClient("mongodb://localhost:27017").GetDatabase("testDB"));
    private readonly ISearchService _searchService = new SearchService(new MongoClient("mongodb://localhost:27017").GetDatabase("testDB"), Mock.Of<ILogger<SearchService>>());
    private readonly ICleanupService _cleanupService = new CleanupService(new MongoClient("mongodb://localhost:27017"), "testDB", Mock.Of<ILogger<ImportService>>());
}