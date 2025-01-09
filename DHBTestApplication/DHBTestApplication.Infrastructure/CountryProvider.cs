using DHBTestApplication.Domain;
using System.Text.Json;

namespace DHBTestApplication.Infrastructure
{
    public class CountryProvider : ICountryProvider
    {
        private HttpClient _httpClient = new();
        public async Task<List<CountryDto>> GetAllCountries()
        {
            var result = await _httpClient.GetAsync("https://localhost:7220/countries");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryDto>>(resultStream);

            return countryList;
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
