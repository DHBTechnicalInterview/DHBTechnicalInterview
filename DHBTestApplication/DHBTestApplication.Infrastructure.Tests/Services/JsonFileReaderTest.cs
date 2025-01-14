using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Infrastructure.Services;
using NUnit.Framework;
using FluentAssertions;

namespace DHBTestApplication.Infrastructure.Tests.Services;

[TestFixture]
[TestOf(typeof(JsonFileReader))]
public class JsonFileReaderTest
{
    private JsonFileReader jsonFileReader;

    [SetUp]
    public void Setup()
    {
        jsonFileReader = new JsonFileReader();
    }
    [Test]
    public async Task ReadJsonFile()
    {
        var result=await jsonFileReader.ReadJsonFileAsync("AllCountries.json");
        result.Should().NotBeNull();
        Action validation = () => JsonSerializer.Deserialize<List<CountryDto>>(result);
        validation.Should().NotThrow("result should be valid JSON");
    }
}