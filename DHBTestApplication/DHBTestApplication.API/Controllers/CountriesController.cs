using DHBTestApplication.Application.Features.Country;
using DHBTestApplication.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DHBTestApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        private readonly GetAllCountries getAllCountries;
        private readonly SearchCountriesByName searchCountriesByName;
        public CountriesController(ILogger<CountriesController> logger,GetAllCountries getAllCountries,SearchCountriesByName searchCountriesByName)
        {
            _logger = logger;
            this.getAllCountries = getAllCountries;
            this.searchCountriesByName = searchCountriesByName;
        }

        [HttpGet("/countries")]
        public async Task<ActionResult> GetCountries()
        {
            try
            {
                var countries = await getAllCountries.getAll();
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
                var countries = await searchCountriesByName.searchByName(countryName);
                if (countries.Count == 0)
                {
                    return NotFound($"No countries found for '{countryName}'");
                }
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching");
                return StatusCode(500, "Error occurred while searching for the country");
            };

        }

    }
}
