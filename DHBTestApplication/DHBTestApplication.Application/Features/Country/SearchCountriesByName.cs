using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interface;

namespace DHBTestApplication.Application.Features.Country;

public class SearchCountriesByName
{
    private ICountryService countryService;
    private ISimilarityService similarityService;
    public SearchCountriesByName(ICountryService countryService, ISimilarityService similarityService)
    {
        this.countryService = countryService;
        this.similarityService = similarityService;
    }
    public async Task<List<CountryDto>> searchByName(string countryName)
    {
        var countries = await countryService.GetAllCountriesAsync();
        return countries
            .Select(country => new
            {
                Country = country,
                //choose the maximum from the common name similarity and offical name similairty
                Similarity = Math.Max(similarityService.CalculateSimilarity( countryName,country.name.common)
                    ,similarityService.CalculateSimilarity(countryName,country.name.official ))
            })
            //threshold set 0.5
            .Where(x => x.Similarity > 0.5)
            .OrderByDescending(x => x.Similarity)
            .Select(x => x.Country)
            .ToList();
    }
}