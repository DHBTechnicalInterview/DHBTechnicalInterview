using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace DHBTestApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly ICountrySearchService _countrySearchService;
        public CountriesController(ILogger<CountriesController> logger,ICountrySearchService countrySearchService)
        {
            _logger = logger;
            _countrySearchService = countrySearchService;
        }

        [HttpGet("/countries")]
        public async Task<ActionResult> GetCountries()
        {
            try
            {
                var countries = await _countrySearchService.GetAllCountriesAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries");
                return StatusCode(500, "An error occurred while retrieving countries");
            }
        }

        [HttpGet("/countries/{countryName}")]
        public async Task<ActionResult> GetCountry(string countryName)
        {
            try
            {
                var countries = await _countrySearchService.SearchCountriesByNameAsync(countryName);
                if (!countries.Any())
                {
                    return NotFound($"No countries found matching '{countryName}'");
                }
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for country: {CountryName}", countryName);
                return StatusCode(500, "An error occurred while searching for the country");
            };

        }

    }
}
