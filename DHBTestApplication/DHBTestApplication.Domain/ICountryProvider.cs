namespace DHBTestApplication.Domain
{
    public interface ICountryProvider
    {
        public Task<CountryDto> GetCountry(string countryName);
        public Task<List<CountryDto>> GetAllCountries();
    }
}
