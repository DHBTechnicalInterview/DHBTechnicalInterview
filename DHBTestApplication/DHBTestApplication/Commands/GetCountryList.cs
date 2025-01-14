using DHBTestApplication.Domain;
using MediatR;
using DHBTestApplication.Web.Clients;

//move the class from application layer to the services directory of the web side
namespace DHBTestApplication.Web.Commands
{
    public class GetCountryListQuery : IRequest<List<Country>>
    {

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
}