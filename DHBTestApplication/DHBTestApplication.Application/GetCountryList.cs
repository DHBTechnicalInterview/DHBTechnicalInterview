using DHBTestApplication.Domain;
using MediatR;

namespace DHBTestApplication.Application
{
    public class GetCountryListQuery : IRequest<List<Country>>
    {

    }
    //Feature: Passing the request query to the query handler in the CQRS pattern.
    public class SearchCountryListQuery : IRequest<List<Country>>
    {
        public string Query { get; }
        
        public SearchCountryListQuery(string query)
        {
            Query = query;
        }
    }

    public class GetCountryListHandler : IRequestHandler<GetCountryListQuery, List<Country>>
    {
        private readonly ICountryProvider provider;

        public GetCountryListHandler(ICountryProvider provider)
        {
            this.provider = provider;
        }
        public async Task<List<Country>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
        {
            var result = await provider.GetAllCountries();
            var countryList = new List<Country>();
            result.ForEach(x => countryList.Add(x.ToCountry()));

            countryList = countryList.OrderByDescending(x => x.CalculateDensity()).ToList();
            return countryList;
        }
    }
    //Feature : Processing the request query in the CQRS pattern
    public class SearchCountryListHandler : IRequestHandler<SearchCountryListQuery, List<Country>>
    {
        private readonly ICountryProvider provider;

        public SearchCountryListHandler(ICountryProvider provider)
        {
            this.provider = provider;
        }
        //Feature : Fetching all the countries and matching it with the Request Query using LINQ.
        public async Task<List<Country>> Handle(SearchCountryListQuery request, CancellationToken cancellationToken)
        {
            var result = await provider.GetAllCountries();
            var countryList = new List<Country>();
            result.ForEach(x => countryList.Add(x.ToCountry()));

            var searchResults = countryList
                .Where(c => c.Name.Contains(request.Query, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(c => c.Name.ToLower().StartsWith(request.Query.ToLower()))
                .ThenBy(c => c.Name.Length)
                .ToList();

            return searchResults;
        }
    }
}
