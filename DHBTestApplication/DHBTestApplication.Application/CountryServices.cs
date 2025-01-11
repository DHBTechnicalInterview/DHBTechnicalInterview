
using System.Text.Json;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interfaces;
using DHBTestApplication.Domain.Services;
using DHBTestApplication.Infrastructure.Services;

namespace DHBTestApplication.Application.Services
{
    public class CountryService : ICountrySearchService
    {
        private readonly IJsonFileReader _jsonFileReader;
        private readonly IStringSimilarityService _similarityService;
        private const string CountriesFilePath = "AllCountries.json";

        public CountryService(
            IJsonFileReader jsonFileReader,
            IStringSimilarityService similarityService)
        {
            _jsonFileReader = jsonFileReader;
            _similarityService = similarityService;
        }

        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            var fileContent = await _jsonFileReader.ReadJsonFileAsync(CountriesFilePath);
            return JsonSerializer.Deserialize<List<CountryDto>>(fileContent);
        }

        public async Task<List<CountryDto>> SearchCountriesByNameAsync(string countryName)
        {
            var countries = await GetAllCountriesAsync();

            return countries
                .Select(country => new
                {
                    Country = country,
                    Similarity = _similarityService.CalculateSimilarity(
                                     country.name.common.ToLower(),
                                     countryName.ToLower())
                                 + _similarityService.CalculateSimilarity(
                                     country.name.official.ToLower(),
                                     countryName.ToLower())
                })
                .Where(x => x.Similarity > 0.3)
                .OrderByDescending(x => x.Similarity)
                .Select(x => x.Country)
                .ToList();
        }
    }
}