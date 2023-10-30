using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace TravelMate
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            InitializeComponent();
        }

        private async void OnGetRouteClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StartCoords.Text) || string.IsNullOrEmpty(EndCoords.Text))
            {
                await DisplayAlert("Error", "Please enter both start and end street addresses.", "OK");
                return;
            }

            // Fetch coordinates for the start street address.
            JObject startLocation = await GeocodingHelper.GetLocation(StartCoords.Text);
            if (startLocation["data"] == null || !startLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the start address.", "OK");
                return;
            }
            double startLat = startLocation["data"][0]["latitude"].Value<double>();
            double startLon = startLocation["data"][0]["longitude"].Value<double>();

            // Fetch coordinates for the end street address.
            JObject endLocation = await GeocodingHelper.GetLocation(EndCoords.Text);
            if (endLocation["data"] == null || !endLocation["data"].HasValues)
            {
                await DisplayAlert("Error", "Could not retrieve location data for the end address.", "OK");
                return;
            }

            double endLat = endLocation["data"][0]["latitude"].Value<double>();
            double endLon = endLocation["data"][0]["longitude"].Value<double>();

            JObject route = await DigitransitHelper.GetRoute(startLat, startLon, endLat, endLon);

            // For demonstration purposes, display the raw JSON.
            ResultEditor.Text = route.ToString();
        }

        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StreetAddressInput.Text))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(StreetAddressInput.Text);

            // For demonstration purposes, display the raw JSON.
            ResultEditor.Text = location.ToString();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            ResultEditor.Text = "";

            if (string.IsNullOrEmpty(StreetAddressInput.Text))
            {
                await DisplayAlert("Error", "Please enter the street address.", "OK");
                return;
            }

            JObject location = await GeocodingHelper.GetLocation(StreetAddressInput.Text);
            if (location["data"] != null && location["data"].HasValues)
            {
                double lat = location["data"][0]["latitude"].Value<double>();
                double lon = location["data"][0]["longitude"].Value<double>();

                JObject weather = await WeatherHelper.GetWeather(lat, lon);

                // For demonstration purposes, display the raw JSON.
                ResultEditor.Text = weather.ToString();
            }
            else
            {
                await DisplayAlert("Error", "Could not retrieve location data.", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
