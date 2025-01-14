using DHBTestApplication.Application;
using DHBTestApplication.Domain;
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

        public string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                _ = SearchCountries(); // Fire and forget
            }
        }
        protected async override Task OnInitializedAsync()
        {
            //Fix: When clicking on Rank all the countries are loaded initially 
            //without waiting for the user to click on the "Load Countries button"
            await LoadCountriesInOrder();
        }
        public async Task LoadCountriesInOrder()
        {
            IsLoaded = false;
            StateHasChanged(); //Fix: Explicitly called as in some cases state change is not being reflected in the UI
            CountryList = await Mediator.Send(new GetCountryListQuery());
            IsLoaded = true;
            StateHasChanged();
        }
        public async Task SearchCountries()
        {
            IsLoaded = false;
            StateHasChanged();
            CountryList = await Mediator.Send(new SearchCountryListQuery(SearchQuery));
            IsLoaded = true;
            StateHasChanged();
        }
    }
}
