using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Features.Country;
using DHBTestApplication.Application.Interface;
using DHBTestApplication.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DHBTestApplication.Application.Tests.Features.Country;

[TestFixture]
[TestOf(typeof(SearchCountriesByName))]
public class SearchCountriesByNameTest
{
    private Mock<IJsonFileReader> jsonFileReaderMock;
    private Mock<ISimilarityService> similarityServiceMock;
    private Mock<ICountryService> countryServiceMock;
    private SearchCountriesByName searchCountriesByName;
    [SetUp]
    public void Setup()
    {
        jsonFileReaderMock = new Mock<IJsonFileReader>();
        similarityServiceMock = new Mock<ISimilarityService>();
        jsonFileReaderMock
            .Setup(x => x.ReadJsonFileAsync(It.IsAny<string>()))
            .ReturnsAsync("[]");

        similarityServiceMock
            .Setup(x => x.CalculateSimilarity(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(0.0);
        countryServiceMock
            .Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(new List<CountryDto>());
        searchCountriesByName = new SearchCountriesByName(countryServiceMock.Object, similarityServiceMock.Object);
    }

    [TestCase("United Kingdom","United Kingdom",1.0,Description = "normal search")]
    [TestCase("United","United Kingdom",1.0,Description = "normal search")]
    [TestCase("UnitedmodgniK","United Kingdom",0.4,Description = "normal search")]
    public async Task SearchCountry(string searchString,string targetCountry,double similarity)
    {
        List<CountryDto> countryList = new List<CountryDto>();
        countryList.Add(new CountryDto(){name=new Name(){common=targetCountry,official = targetCountry}});
        jsonFileReaderMock.Setup(x=>x.ReadJsonFileAsync(It.IsAny<string>())).ReturnsAsync(JsonSerializer.Serialize(countryList));
        similarityServiceMock.Setup(x=>x.CalculateSimilarity(It.IsAny<string>(), It.IsAny<string>())).Returns(similarity);
        var result = await searchCountriesByName.searchByName(searchString);
        if (similarity > 0.5)
        {
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            result[0].name.common.Should().Be(targetCountry);
        }
        else
        {
            result.Should().BeEmpty();
        }
    }
}