using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interface;

namespace DHBTestApplication.Application.Features.Country;

public class GetAllCountries
{
   private ICountryService countryService;

   public GetAllCountries(ICountryService countryService)
   {
      this.countryService = countryService;
   }

   public async Task<List<CountryDto>> getAll()
   {
      try
      {
      List<CountryDto>countries=await countryService.GetAllCountriesAsync();
      return countries;
      }
      catch (Exception e)
      {
         Console.WriteLine("country service have error:",e);
         return new List<CountryDto>();
         throw;
      }
   }

}