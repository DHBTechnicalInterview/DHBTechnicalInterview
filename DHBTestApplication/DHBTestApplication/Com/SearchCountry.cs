using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Services;
using MediatR;
using System.Text.Json;
using DHBTestApplication.Web.Dto;

namespace DHBTestApplication.Web.Utils
{
    public class SearchCountryQuery : IRequest<List<CountryDto>>
    {
        public string SearchTerm { get; set; }
        public SearchCountryQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }

    public class SearchCountryHandler : IRequestHandler<SearchCountryQuery, List<CountryDto>>
    {
        private readonly ICountryProvider _provider;
        public SearchCountryHandler(ICountryProvider provider)
        {
            _provider = provider;
        }

        public async Task<List<CountryDto>> Handle(SearchCountryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _provider.GetCountry(request.SearchTerm);
                return result;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
              return null;
            }
        }
    }
}