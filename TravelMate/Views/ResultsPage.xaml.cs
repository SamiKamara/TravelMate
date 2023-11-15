using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
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
            viewModel = new ResultsPageViewModel(routeSettings);
            BindingContext = viewModel;
        }

        protected async void OnGetRouteClicked(object sender, EventArgs e)
        {
            await viewModel.GetRouteAsync(ResultEditor, DesiredWeather, DestinationWeather);
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
            await viewModel.GetRouteAsync(ResultEditor, DesiredWeather, DestinationWeather);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out int result))
            {
                ((Entry)sender).Text = e.OldTextValue;
                return;
            }

            if (result < 0 || result > 100)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}
