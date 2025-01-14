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
        public string SearchQuery { get; set; }
        public string ErrorMessage { get; set; }
        public NavigationManager NavigationManager { get; set; }
        protected async override Task OnInitializedAsync()
        {
            //Fix: When clicking on Rank all the countries are loaded initially 
            //without waiting for the user to click on the "Load Countries button"
            await LoadCountriesInOrder();
        }
        public async Task LoadCountriesInOrder()
        {
            try{
                IsLoaded = false;
                StateHasChanged(); //Fix: Explicitly called as in some cases state change is not being reflected in the UI
                CountryList = await Mediator.Send(new GetCountryListQuery());
                IsLoaded = true;
                ErrorMessage = null;
                StateHasChanged();
            }catch(Exception ex){
                IsLoaded = true;
                ErrorMessage = ex.Message;
                StateHasChanged();
                throw;
            }
        }
        //Feature : Method to search for each key value as it is being typed
        public async Task OnSearchChange(string value)
        {
            try
            {
                await InvokeAsync(async () =>
                {
                    IsLoaded = false;
                    StateHasChanged();

                    SearchQuery = value;
                    CountryList = await Mediator.Send(new SearchCountryListQuery(SearchQuery));
                    
                    IsLoaded = true;
                    ErrorMessage = null;
                    StateHasChanged();
                });
            }
            catch (Exception ex)
            {
                // Add proper error handling here
                IsLoaded = true;
                ErrorMessage = ex.Message;
                StateHasChanged();
                throw;
            }
        }
    }
}
