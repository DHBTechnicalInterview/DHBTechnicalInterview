using System.Text.Json;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interface;
using DHBTestApplication.Domain.Services;
using DHBTestApplication.Infrastructure.Services;
using ISimilarityService = DHBTestApplication.Application.Interface.ISimilarityService;

namespace DHBTestApplication.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IJsonFileReader _jsonFileReader;
        private readonly ISimilarityService _similarityService;
        private const string CountriesFilePath = "AllCountries.json";

        public CountryService(
            IJsonFileReader jsonFileReader,
            ISimilarityService similarityService)
        {
            _jsonFileReader = jsonFileReader;
            _similarityService = similarityService;
        }

        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            var fileContent = await _jsonFileReader.ReadJsonFileAsync(CountriesFilePath);
            return JsonSerializer.Deserialize<List<CountryDto>>(fileContent);
        }



    }
}