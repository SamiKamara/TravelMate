using Microsoft.Maui.Controls;

namespace TravelMate
{
    public partial class WeatherPage : ContentPage
    {
        public WeatherPage()
        {
            InitializeComponent();
        }

        private void OnViewResultsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResultsPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
