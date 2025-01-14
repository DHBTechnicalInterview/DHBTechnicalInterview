using DHBTestApplication.Application.Dto;
using DHBTestApplication.Application.Features.Country;
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
public class SearchCountriesByNameTest
{
    private  static ServiceProvider serviceProvider;
    private  static SearchCountriesByName searchCountriesByName;
    private  static ICountryService countryService;
    private  static ISimilarityService similarityService;
    private  static readonly object _lock = new object();
    private  static List<TestCaseData> testCases;
    private static bool initializedLock;

    private static void EnsureInitialized()
    {
        if (initializedLock) return;
        lock (_lock)
        {
            if (initializedLock) return;
            try
            {
                ServiceCollection services = new ServiceCollection();
                services.AddScoped<IJsonFileReader, JsonFileReader>();
                services.AddScoped<ISimilarityService, SimilarityService>();
                services.AddScoped<ICountryService, CountryService>();
                services.AddScoped<TotalTestCases>();
                services.AddScoped<SearchCountriesByName>();
                serviceProvider = services.BuildServiceProvider();
                countryService = serviceProvider.GetRequiredService<ICountryService>();
                similarityService = serviceProvider.GetRequiredService<ISimilarityService>();
                searchCountriesByName = serviceProvider.GetRequiredService<SearchCountriesByName>();
                var generator = serviceProvider.GetRequiredService<TotalTestCases>();
                testCases = generator.GenerateTestData().GetAwaiter().GetResult().ToList<TestCaseData>();
                initializedLock = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"test data genertation failed: {ex.Message}", ex);
            }
        }
    }

    public static IEnumerable<TestCaseData> GetTestCases()
    {
        EnsureInitialized();
        return testCases;
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        EnsureInitialized();
    }



    [TestCaseSource(typeof(SampleTestCases), nameof(SampleTestCases.SearchTestCasesExisted))]
    public async Task SimilarityTestExisted(string searchString, string targetCountry)
    {
        var response = await searchCountriesByName.searchByName(searchString);
        var results = response.Any(x => x.name.official == targetCountry || x.name.common == targetCountry);
        Console.WriteLine($"search string: \"{searchString}\"");
        Console.WriteLine($"target country : \"{targetCountry}\"");
        Console.WriteLine($"maximum similarity: {similarityService.CalculateSimilarity(searchString, targetCountry)}");
        Console.WriteLine($"whether it exists in search response: {(bool)results}");
        Console.WriteLine($"number of counties in search response: {response.Count()}");
        Console.WriteLine($"############################################################");
        for (int i = 0; i < response.Count; i++)
        {
            var result = response[i];
            Console.WriteLine($"{i+1}: {result.name.official}, {result.name.common}, similarity: {Math.Max(similarityService.CalculateSimilarity(searchString, result.name.common), similarityService.CalculateSimilarity(searchString, result.name.official))}");
        }
        var countriesWithSimilarity = response.Select(country => new
        {
            Country = country,
            Similarity = Math.Max(
                similarityService.CalculateSimilarity(searchString, country.name.common),
                similarityService.CalculateSimilarity(searchString, country.name.official)
            )
        }).ToList();

        countriesWithSimilarity.Should().BeInDescendingOrder(item => item.Similarity);

        results.Should().BeTrue();
    }

    [TestCaseSource(typeof(SampleTestCases), nameof(SampleTestCases.SearchTestCasesNotExisted))]
    public async Task SimilarityTestNotExisted(string searchString, string targetCountry)
    {
        var response = await searchCountriesByName.searchByName(searchString);
        var results = response.Any(x => x.name.official == targetCountry || x.name.common == targetCountry);
        Console.WriteLine($"search string: \"{searchString}\"");
        Console.WriteLine($"target country : \"{targetCountry}\"");
        Console.WriteLine($"maximum similarity: {similarityService.CalculateSimilarity(searchString, targetCountry)}");
        Console.WriteLine($"whether it exists in search response: {(bool)results}");
        results.Should().BeFalse();
    }

    [TestCaseSource(typeof(SampleTestCases), nameof(SampleTestCases.SearchTestCasesSecurity))]
    public async Task SimilarityTestSecurity(string searchString, string targetCountry)
    {
        var response = await searchCountriesByName.searchByName(searchString);
        var results = response.Any(x => x.name.official == targetCountry || x.name.common == targetCountry);
        Console.WriteLine($"search string: \"{searchString}\"");
        Console.WriteLine($"whether it exists in search response: {(bool)results}");
        results.Should().BeFalse();
    }

    [TestCaseSource(nameof(GetTestCases))]
    public async Task SimilarityTestTotal(string searchString, Name targetCountryName)
    {
        var response = await searchCountriesByName.searchByName(searchString);
        bool exists = response.Any(x => x.name.common == targetCountryName.common);
        double similarityCommon = similarityService.CalculateSimilarity(searchString, targetCountryName.common);
        double similarityOffical = similarityService.CalculateSimilarity(searchString, targetCountryName.official);
        double similarity = Math.Max(similarityCommon, similarityOffical);
        Console.WriteLine($"search string: \"{searchString}\"");
        Console.WriteLine($"target country common name: \"{targetCountryName.common}\"");
        Console.WriteLine($"target country official name: \"{targetCountryName.official}\"");
        Console.WriteLine($"similarity for common name: {similarityCommon}");
        Console.WriteLine($"similarity for offical name: {similarityOffical}");
        Console.WriteLine($"maximum similarity: {similarity}");
        Console.WriteLine($"whether it exists in search response: {exists}");
        Console.WriteLine($"############################################################");
        string mark = "*";
        if (exists)
        {
            for (int i = 0; i < response.Count; i++)
            {
                var result = response[i];
                Console.WriteLine($"{(result.name.common == targetCountryName.common ? mark : string.Empty)}" +
                                  $"{i+1}: {result.name.official}, {result.name.common}, similarity: {Math.Max(similarityService.CalculateSimilarity(searchString, result.name.common), similarityService.CalculateSimilarity(searchString, result.name.official))}");
            }

            var countriesWithSimilarity = response.Select(country => new
            {
                Country = country,
                Similarity = Math.Max(
                    similarityService.CalculateSimilarity(searchString, country.name.common),
                    similarityService.CalculateSimilarity(searchString, country.name.official)
                )
            }).ToList();

            countriesWithSimilarity.Should().BeInDescendingOrder(item => item.Similarity);

            similarity.Should().BeGreaterThan(0.5,$"\"{searchString}\" has similarity of {similarityCommon} for \"{targetCountryName.common}\"" +
                                                  $"\n \"{searchString}\"  has similarity of {similarityOffical} for \"{targetCountryName.official}\"" +
                                                  $"\n  \"{searchString}\" Max similarity is {similarity}. ");
        }
        else
        {
            similarity.Should().BeLessOrEqualTo(0.5,$"\"{searchString}\" has similarity of {similarityCommon} for \"{targetCountryName.common}\"" +
                                                  $"\n \"{searchString}\"   has similarity of {similarityOffical} for \"{targetCountryName.official}\"" +
                                                  $"\n  \"{searchString}\"  Max similarity is {similarity}. ");
        }
    }


}