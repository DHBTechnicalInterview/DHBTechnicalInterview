using DHBTestApplication.Application.Interface;
using DHBTestApplication.Application.Services;
using DHBTestApplication.Domain.Services;
using DHBTestApplication.Infrastructure.Services;
using FluentAssertions;
using IntergartionTest.Feature.Country.TestCases;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IntergartionTest.Feature.Country;

[TestFixture]
public class GetAllContriesTests
{
    private static IServiceProvider serviceProvider;
    private static ICountryService countryService;
    private static ISimilarityService similarityService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddScoped<IJsonFileReader, JsonFileReader>();
        services.AddScoped<ISimilarityService, SimilarityService>();
        services.AddScoped<ICountryService, CountryService>();

        serviceProvider = services.BuildServiceProvider();
        countryService = serviceProvider.GetRequiredService<ICountryService>();
        similarityService = serviceProvider.GetRequiredService<ISimilarityService>();
    }

    [Test]
    public async Task GetAllCountries()
    {
        var result=await countryService.GetAllCountriesAsync();
        result.Should().NotBeNullOrEmpty();
        Console.WriteLine($"the number of countries : {result.Count()}");
    }

}




