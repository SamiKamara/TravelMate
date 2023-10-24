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
            if (string.IsNullOrEmpty(StartCoords.Text) || string.IsNullOrEmpty(EndCoords.Text))
            {
                await DisplayAlert("Error", "Please enter both start and end coordinates.", "OK");
                return;
            }

            string[] startCoords = StartCoords.Text.Split(',');
            string[] endCoords = EndCoords.Text.Split(',');

            JObject route = await DigitransitHelper.GetRoute(
                double.Parse(startCoords[0].Trim()),
                double.Parse(startCoords[1].Trim()),
                double.Parse(endCoords[0].Trim()),
                double.Parse(endCoords[1].Trim())
            );

            // For demonstration purposes, display the raw JSON.
            RouteLabel.Text = route.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
