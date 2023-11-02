using Microsoft.Maui.Controls;
using TravelMate.Services;

namespace TravelMate
{   
    public partial class WeatherPage : ContentPage
    {
        private UserSettingsService userSettingsService;
        public WeatherPage(UserSettingsService routeSettings)
        {
            InitializeComponent();

            userSettingsService = routeSettings;

            this.BindingContext = userSettingsService;
            
        }

        private void OnViewResultsClicked(object sender, EventArgs e)
        { 
            Navigation.PushAsync(new ResultsPage(userSettingsService));
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
