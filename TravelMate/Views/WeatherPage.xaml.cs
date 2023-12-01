using Microsoft.Maui.Controls;
using TravelMate.Services;

namespace TravelMate
{   
    public partial class WeatherPage : ContentPage
    {
        private WeatherPageViewModel viewModel;
        public WeatherPage(UserSettingsService routeSettings)
        {
            InitializeComponent();

            viewModel = new WeatherPageViewModel(routeSettings);

            BindingContext = viewModel;
            
        }

        private async void OnViewResultsClicked(object sender, EventArgs e)
        { 
            await Navigation.PushAsync(new ResultsPage(viewModel.RouteData));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}
