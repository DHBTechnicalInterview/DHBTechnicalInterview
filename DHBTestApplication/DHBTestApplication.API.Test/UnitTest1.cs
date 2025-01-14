using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DHBTestApplication.API.Test;
public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
        public async Task GetAllCountries_ReturnsOkResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/countries");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    
    [Fact]
        public async Task GetNonExistentEndpoint_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/nonexistent");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        
}