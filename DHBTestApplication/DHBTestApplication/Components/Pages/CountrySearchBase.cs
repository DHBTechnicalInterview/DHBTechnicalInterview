using System.Linq.Dynamic.Core.Parser;
using System.Text.RegularExpressions;
using DHBTestApplication.Web.Payload;
using DHBTestApplication.Web.Commands;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace DHBTestApplication.Web.Components.Pages
{
    public class CountrySearchBase : ComponentBase
    {
        [Inject]
        protected IMediator Mediator { get; set; }

        private string searchTerm = "";
        protected List<CountryPayload> Countries { get; set; } = new List<CountryPayload>();
        protected string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                WarningMessage = "";
                ErrorMessage = "";
                StateHasChanged();
                Countries.Clear();
                try
                {
                if(string.IsNullOrWhiteSpace(value)&&value.Length>0)WarningMessage = "Please enter keywords for your search.";
                else if(value.Length >0&&value.Length<3)WarningMessage = "Please enter more words.";
                else if(value.Length>25)WarningMessage = "The search words limit is 25 characters.";
                else if((double)Regex.Matches(value,"[^a-zA-Z]").Count/(double)value.Length>=0.6)WarningMessage = "Please enter correct words.";
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception for search string:",e);
                    throw;
                }
            }
        }
        protected string ErrorMessage { get; set; } = "";
        protected string WarningMessage { get; set; } = "";
        protected async Task OnSearchInput(ChangeEventArgs e)
        {
            //ensure not null
            SearchTerm = e.Value?.ToString() ?? "";
            //after setting searchTerm, cancel for making unnecessary requests
            if(SearchTerm.Length == 0 || WarningMessage.Length>0)return;
            try
            {
                ErrorMessage = "";
                var results = await Mediator.Send(new SearchCountryQuery(SearchTerm));
                Countries = results;

                if (!Countries.Any())
                {
                    WarningMessage = "No countries found, please enter correct keywords.";
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error search. Please try again."+ex.Message;
                Countries = new List<CountryPayload>();
                StateHasChanged();
            }

        }
    }
}

