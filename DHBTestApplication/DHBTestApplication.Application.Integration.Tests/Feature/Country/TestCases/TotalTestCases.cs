using System.Text;
using DHBTestApplication.Application.Dto;
using DHBTestApplication.Infrastructure.Services;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntergartionTest.Feature.Country.TestCases;

public class TotalTestCases
{
    private readonly IJsonFileReader _jsonFileReader;
    private readonly Random _random = new Random();


    public TotalTestCases(IJsonFileReader jsonFileReader)
    {
        _jsonFileReader = jsonFileReader;
    }

    public  async Task<IEnumerable<TestCaseData>> GenerateTestData()
    {
        var formalData=await _jsonFileReader.ReadJsonFileAsync("AllCountries.json");
        var countries = JsonSerializer.Deserialize(formalData, typeof(IEnumerable<CountryDto>)) as IEnumerable<CountryDto>;
        var testCaseData = new List<TestCaseData>();

        foreach (CountryDto country in countries)
        {
            testCaseData=testCaseData.Concat(Generator(country)).ToList();
        }
        return testCaseData;
    }

    private List<TestCaseData> Generator(CountryDto country)
    {



        string searchString = _random.Next(1, 3) == 1 ?country.name.common:country.name.official;
        Name targetCountryName = country.name;

        List<TestCaseData> testdata = new List<TestCaseData>();

        testdata.Add(new TestCaseData(searchString.ToUpper(), targetCountryName));
        testdata.Add(new TestCaseData(searchString.ToLower(), targetCountryName));
        testdata.Add(new TestCaseData(GenerateString(searchString,SimilarMap()), targetCountryName));
        testdata.Add(new TestCaseData(GenerateString(searchString,new System.Collections.Generic.Dictionary<char, char[]>(){}), targetCountryName));
        return testdata;
    }

    private string GenerateString(string searchString,Dictionary<char,char[]> map)
    {
        StringBuilder searchStringBuilder = new StringBuilder(searchString);
        int blockLength = _random.Next((searchString.Length +4)/ 5 , searchString.Length);
        for (int i = 0; i < searchString.Length/blockLength; i++)
        {
            int randomIndex=_random.Next(blockLength*i,Math.Min(searchString.Length, blockLength*(i+1)));
            searchStringBuilder[randomIndex]=ReplacedChar(searchStringBuilder[randomIndex],map);
        }
        return searchStringBuilder.ToString();
    }

    private char ReplacedChar(char u, Dictionary<char, char[]> map)
    {
        const string specialChars = "&!@#$%^&*()_.,+±|}{?><\"\'0123456789";
        if (map.ContainsKey(u)) return map[u][_random.Next(map[u].Length)];
        return specialChars[_random.Next(specialChars.Length)];
    }

    private Dictionary<char,char[]> SimilarMap()
    {
        var map = new Dictionary<char, char[]>
        {
            {'a', new[] {'e', 'q', 'w', 's', 'z'}},
            {'e', new[] {'r', 'w', 's', 'd', 'f'}},
            {'i', new[] {'o', 'k', 'j', 'u'}},
            {'o', new[] {'i', 'p', 'k', 'l'}},
        };
        return map;
    }




}