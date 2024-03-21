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
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (08_06_2019).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (09_08_2017) LCI535.csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (11_05_2022).csv");

        // Act
        var parameterRequested = "FirmwareVersionLIN7_000";
        var result = _searchService.SearchEverywhere(new string[] { "1081" }, new[] { parameterRequested });

        // Assert
        result.Count.ShouldBe(1);
        var searchResult = result.First();
        searchResult.Name.ShouldBe("1081");
        searchResult.Parameters.Count.ShouldBe(1);
        var parameter = searchResult.Parameters.First();
        parameter.Name.ShouldBe(parameterRequested);
        parameter.Unit.ShouldBeEmpty();
        parameter.QualifiedName.ShouldBe("12608");
        parameter.Value.ShouldBe("0");
        parameter.Unit.ShouldBeEmpty();
    }
    
    [Test]
    public void Search_ValidInput_QueryMultipleParameters()
    {
        // Arrange
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (08_06_2019).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (09_08_2017) LCI535.csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (11_05_2022).csv");

        // Act
        var parameterRequested = "FirmwareVersionLIN7_000";
        var parameterRequested2 = "AngleOffsetPulseMode_104";
        var result = _searchService.SearchEverywhere(new string[] { "1081" }, new[] { parameterRequested, parameterRequested2 });

        // Assert
        result.Count.ShouldBe(1);
        var searchResult = result.First();
        searchResult.Name.ShouldBe("1081");
        searchResult.Parameters.Count.ShouldBe(2);
        var parameter = searchResult.Parameters.First();
        parameter.Name.ShouldBe(parameterRequested);
        parameter.Unit.ShouldBeEmpty();
        parameter.QualifiedName.ShouldBe("12608");
        parameter.Value.ShouldBe("0");
        parameter.Unit.ShouldBeEmpty();
    }
    
    [Test]
    public void GetAllParameters()
    {
        // Arrange
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (07_08_2023).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (08_06_2019).csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (09_08_2017) LCI535.csv");
        _importService.ImportCsvData("Resources/CT Snapshot Dev AC 800PEC (11_05_2022).csv");

        // Act
        var result = _searchService.GetAllParameters();

        // Assert
        result.Count().ShouldBe(18188);
        result.ShouldContain("AngleOffsetPulseMode_102");
    }
}