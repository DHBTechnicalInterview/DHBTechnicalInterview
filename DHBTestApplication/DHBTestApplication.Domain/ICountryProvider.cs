namespace DHBTestApplication.Domain
{
    public interface ICountryProvider
    {
        public Task<List<CountryDto>> GetAllCountries();
    }
}
