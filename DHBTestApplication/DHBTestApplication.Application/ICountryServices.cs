using DHBTestApplication.Application.Dto;



namespace DHBTestApplication.Application.Interfaces
{
    public interface ICountrySearchService
    {
        Task<List<CountryDto>> GetAllCountriesAsync();
        Task<List<CountryDto>> SearchCountriesByNameAsync(string countryName);
    }
}