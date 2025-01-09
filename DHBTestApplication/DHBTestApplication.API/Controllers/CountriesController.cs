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
        {
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
        }
    }
}
