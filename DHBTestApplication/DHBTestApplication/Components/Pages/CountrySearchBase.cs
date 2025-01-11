using DHBTestApplication.Web.Dto;
using DHBTestApplication.Infrastructure;
using DHBTestApplication.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace DHBTestApplication.Web.Components.Pages
{
    public class CountrySearchBase : ComponentBase
    {
        [Inject]
        public IMediator Mediator { get; set; }

        protected string SearchTerm { get; set; } = "";
        protected List<CountryDto> Countries { get; set; } = new List<CountryDto>();
        protected string ErrorMessage { get; set; } = "";

        protected async Task CountrySearch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    ErrorMessage = "Please enter a country name";
                    return;
                }

                ErrorMessage = "";
                Countries = await Mediator.Send(new SearchCountryQuery(SearchTerm));

                if (Countries == null || !Countries.Any())
                {
                    ErrorMessage = "No countries found";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error searching for country: " + ex.Message;
                Countries = null;
            }
        }
    }
}