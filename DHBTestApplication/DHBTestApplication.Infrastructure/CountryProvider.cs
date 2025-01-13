using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DHBTestApplication.Domain;
using System.Text.Json;

namespace DHBTestApplication.Infrastructure
{
    public class CountryProvider : ICountryProvider
    {
        //Fix: Added HttpClient as Dependency Injection
        private readonly HttpClient _httpClient;
        //Using API URL from configuration file
        private readonly string _apiUrl;
        //Fix : Added Logging for the file
        private readonly ILogger<CountryProvider> _logger;
        public CountryProvider(HttpClient httpClient, IConfiguration configuration, ILogger<CountryProvider> logger)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"];
            _logger = logger;
        }
        public async Task<List<CountryDto>> GetAllCountries()
        {
            //Fix : adding error handling
            try{
            var result = await _httpClient.GetAsync($"{_apiUrl}/countries");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryDto>>(resultStream);
            return countryList;
            }catch(Exception ex){
                _logger.LogError(ex, $"Error fetching countries");
                throw new ApplicationException("Error Fetching Countries", ex);
            }
        }

        public async Task<CountryDto> GetCountry(string countryName)
        {
            var result = await _httpClient.GetAsync($"https://localhost:7220/countries/{countryName}");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<CountryDto>(resultStream);

            return countryList;
        }
    }
}
