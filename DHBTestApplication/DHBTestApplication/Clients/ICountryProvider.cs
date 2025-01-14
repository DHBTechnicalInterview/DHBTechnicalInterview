// put ConturyProvider and its interface in the same layer

using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Payload;

namespace DHBTestApplication.Web.Clients
{
    public interface ICountryProvider
    {
        public Task<List<CountryPayload>> GetCountry(string countryName);
        public Task<List<CountryPayload>> GetAllCountries();
    }
}
