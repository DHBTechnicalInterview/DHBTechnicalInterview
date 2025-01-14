using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interface;
using DHBTestApplication.Application.Services;
using DHBTestApplication.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;


namespace DHBTestApplication.Application.Tests.Features.Country
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<IJsonFileReader> jsonFileReaderMock;
        private Mock<ISimilarityService> similarityServiceMock;
        private CountryService countryService;

        [SetUp]
        public void Setup()
        {
            jsonFileReaderMock = new Mock<IJsonFileReader>();
            similarityServiceMock = new Mock<ISimilarityService>();
            countryService = new CountryService(jsonFileReaderMock.Object, similarityServiceMock.Object);
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnDeserializedCountries_WhenJsonIsValid()
        {
            var countriesJson = @"[
                {""name"": {""common"": ""United States"", ""official"": ""United States of America""}},
                {""name"": {""common"": ""Canada"", ""official"": ""Canada""}}
            ]";
            jsonFileReaderMock.Setup(x => x.ReadJsonFileAsync("AllCountries.json")).ReturnsAsync(countriesJson);

            var result = await countryService.GetAllCountriesAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].name.common.Should().Be("United States");
            result[0].name.official.Should().Be("United States of America");
            result[1].name.common.Should().Be("Canada");
            result[1].name.official.Should().Be("Canada");
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnEmptyList_WhenJsonIsEmpty()
        {
            var emptyJson = "[]";
            jsonFileReaderMock.Setup(x => x.ReadJsonFileAsync("AllCountries.json")).ReturnsAsync(emptyJson);

            var result = await countryService.GetAllCountriesAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldCallReadJsonFileAsync_WithCorrectFilePath()
        {
            var countriesJson = "[]";
            jsonFileReaderMock.Setup(x => x.ReadJsonFileAsync("AllCountries.json")).ReturnsAsync(countriesJson);

            await countryService.GetAllCountriesAsync();

            jsonFileReaderMock.Verify(x => x.ReadJsonFileAsync("AllCountries.json"), Times.Once);
        }

        [Test]
        public void GetAllCountriesAsync_ShouldThrowException_WhenJsonIsInvalid()
        {
            var invalidJson = "{invalid_json}";
            jsonFileReaderMock.Setup(x => x.ReadJsonFileAsync("AllCountries.json")).ReturnsAsync(invalidJson);

            Func<Task> act = async () => await countryService.GetAllCountriesAsync();
            act.Should().ThrowAsync<JsonException>();
        }
    }
}