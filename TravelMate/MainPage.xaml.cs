using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using TravelMate.Services;

namespace TravelMate
{
    public partial class MainPage
    {
        private UserSettingsService routeData;
        public MainPage()
        {
            InitializeComponent();

            // Create an instance of the UserSettingsService
            routeData = new UserSettingsService();

            // Set the binding context to the UserSettingsService instance
            this.BindingContext = routeData;
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {

            routeData.From = routeData.From.Trim();
            routeData.To = routeData.To.Trim();

            if (string.IsNullOrEmpty(routeData.From) || string.IsNullOrEmpty(routeData.To))
            {
                await DisplayAlert("Error", "Please enter both start and end street addresses.", "OK");
                return;
            }

            // Temporary solution for start address validation

            JObject startLocation = await GeocodingHelper.GetLocation(routeData.From);
            if (startLocation["data"] == null || !startLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the start address.", "OK");
                return;
            }

            // Temporary solution for destination address validation

            JObject endLocation = await GeocodingHelper.GetLocation(routeData.To);
            if (endLocation["data"] == null || !endLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the destination address.", "OK");
                return;
            }

            await Navigation.PushAsync(new WeatherPage(routeData));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
