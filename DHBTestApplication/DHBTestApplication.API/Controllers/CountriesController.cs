using Microsoft.AspNetCore.Mvc;

namespace DHBTestApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;
        public CountriesController(ILogger<CountriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/countries")]
        public async Task<ActionResult> GetCountries()
        {   //Fix :  Exception Handling
            try{
            var jsonFile = new StreamReader("AllCountries.json");
            var newLine = jsonFile.ReadLine();
            var fileContent = newLine; 
            while (newLine != null)
            {
                //Read the next line
                newLine = jsonFile.ReadLine();
                fileContent += newLine;
            }

            return Ok(fileContent);
            }catch(Exception ex){
                _logger.LogError("Exception when reading from countryList: {Exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
