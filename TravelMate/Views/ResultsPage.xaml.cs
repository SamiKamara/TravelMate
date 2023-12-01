using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TravelMate.Model;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public partial class ResultsPage
    {
        private ResultsPageViewModel viewModel;

        public ResultsPage(UserSettingsService routeSettings)
        {
            InitializeComponent();
            
            viewModel = new ResultsPageViewModel(routeSettings)
            {
                OnRouteSelected = NavigateToRouteDetails
            };

            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            _ = OnResultsPageLoadedAsync();
        }
        private async Task OnResultsPageLoadedAsync()
        {
            try
            {
                await OnResultsPageLoaded();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task OnResultsPageLoaded()
        {
            await viewModel.GetRouteAsync();
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void NavigateToRouteDetails(RouteModel routeModel)
        {
            await Navigation.PushAsync(new DetailedPage());
        }
    }
}
