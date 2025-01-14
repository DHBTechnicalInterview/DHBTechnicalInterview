using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Clients;
using MediatR;
using System.Text.Json;
using DHBTestApplication.Web.Payload;

namespace DHBTestApplication.Web.Commands
{
    public class SearchCountryQuery : IRequest<List<CountryPayload>>
    {
        public string SearchTerm { get; set; }
        public SearchCountryQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }

    public class SearchCountryHandler : IRequestHandler<SearchCountryQuery, List<CountryPayload>>
    {
        private readonly ICountryProvider _provider;
        public SearchCountryHandler(ICountryProvider provider)
        {
            _provider = provider;
        }

        public async Task<List<CountryPayload>> Handle(SearchCountryQuery request, CancellationToken cancellationToken)
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