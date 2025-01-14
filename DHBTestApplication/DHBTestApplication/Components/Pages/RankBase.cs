using DHBTestApplication.Domain;
using DHBTestApplication.Web.Commands;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace DHBTestApplication.Web.Components.Pages
{
    public class RankBase : ComponentBase
    {
        public bool IsLoaded { get; set; } = true;
        public List<Country> CountryList { get; set; }
        [Inject]
        public IMediator Mediator { get; set; }

        protected async override Task OnInitializedAsync()
        {
            
        }
        public async Task LoadCountriesInOrder()
        {
            IsLoaded = false;
            CountryList = await Mediator.Send(new GetCountryListQuery());
            IsLoaded = true;
        }
    }
}
