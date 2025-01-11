// put ConturyProvider and its interface in the same layer

using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Dto;

namespace DHBTestApplication.Web.Services
{
    public interface ICountryProvider
    {
        public Task<List<CountryDto>> GetCountry(string countryName);
        public Task<List<CountryDto>> GetAllCountries();
    }
}
