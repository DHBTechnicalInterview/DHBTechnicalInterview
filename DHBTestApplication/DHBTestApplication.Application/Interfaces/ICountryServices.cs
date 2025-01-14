using DHBTestApplication.Application.Dto;

namespace DHBTestApplication.Application.Interface
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
        // Task<List<CountryDto>> SearchCountriesByName(string countryName);
    }
}