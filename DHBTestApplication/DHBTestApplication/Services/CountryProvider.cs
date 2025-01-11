using System.Text.Json;
using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Dto;

// put ConturyProvider and its interface in the same layer
namespace DHBTestApplication.Web.Services
{
    public class CountryProvider : ICountryProvider
    {
        private HttpClient _httpClient = new();
        public async Task<List<CountryDto>> GetAllCountries()
        {
            var result = await _httpClient.GetAsync("https://localhost:7140/countries");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryDto>>(resultStream);

            return countryList;
        }

        public async Task<List<CountryDto>> GetCountry(string countryName)
        {
            var result = await _httpClient.GetAsync($"https://localhost:7140/countries/{countryName}");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryDto>>(resultStream);

            return countryList;
        }
    }
}
