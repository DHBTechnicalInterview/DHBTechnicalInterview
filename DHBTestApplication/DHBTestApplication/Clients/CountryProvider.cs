using System.Text.Json;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Payload;
using DHBTestApplication.Web.Clients;

//fix bug: 1.the Provider logic is not in the web layer before, and move it into there.
//the architecture is not clear before, it seems like using separation of front-end and back-end pattern
//but the presentation call application directly which like frontend and backend coupling parttern.
//For the separation of frontend and backend,it shoule be the client for handling fetching data request
//in the presentation layer,so it is better be put here.
//If it is not clear, it will have dependency cycle in the later development.
//2.And the Interface for this service is optional, it could be deleted or left, as the interfaces
//of services in the frontend are more about data validation.
namespace DHBTestApplication.Web.Clients
{
    public class CountryProvider : ICountryProvider
    {
        private HttpClient _httpClient = new();
        public async Task<List<CountryPayload>> GetAllCountries()
        {
            //fix bug: the port is not the port of the backend before.
            var result = await _httpClient.GetAsync("https://localhost:7140/countries");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryPayload>>(resultStream);

            return countryList;
        }

        public async Task<List<CountryPayload>> GetCountry(string countryName)
        {
            var result = await _httpClient.GetAsync($"https://localhost:7140/countries/{countryName}");
            var resultStream = await result.Content.ReadAsStreamAsync();
            var countryList = await JsonSerializer.DeserializeAsync<List<CountryPayload>>(resultStream);
            return countryList;
        }
    }
}
